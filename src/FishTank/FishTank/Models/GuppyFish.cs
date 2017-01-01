//
//  Copyright 2017 James Finlay
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using FishTank.Components;
using FishTank.Content;
using FishTank.Instrumentation;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    /// <summary>
    /// Class instance for the guppy fish. This is the simplest fish available in the game. It
    /// chases food, drops coins, and dies.
    /// </summary>
    public class GuppyFish : EconomicFish
    {
        /// <summary>
        /// Basic fish that chases food and dies
        /// </summary>
        /// <param name="graphicsDevice">Graphics resource for texture creation</param>
        public GuppyFish() : base()
        {
            Log.LogVerbose("Creating guppy fish");

            _dropCoinTime = TimeSpan.FromSeconds(15);
            _maxHunger = 1.0f;
            _coinValue = Coin.SilverCoinValue;
            CurrentHunger = _maxHunger;

            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle2(_swimArea.X + Constants.VirtualWidth / 2, 100, 70, 60);

            _moveAnimationHealthy= new Animation(_animationFramesHealthy);
            _moveAnimationHungry = new Animation(_animationFramesHungry);
            _moveAnimationStarving = new Animation(_animationFramesStarving);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_healthyAssetName);
            ContentBuilder.Instance.LoadTextureByName(_hungryAssetName);
            ContentBuilder.Instance.LoadTextureByName(_starvingAssetName);
            ContentBuilder.Instance.LoadTextureByName(_deadAssetName);
            ContentBuilder.Instance.CreateRectangleTexture(_healthyAssetName2, BoundaryBox.ToRectangle().Width, BoundaryBox.ToRectangle().Height);
            _animationFramesHealthy.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            _animationFramesHungry.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            _animationFramesStarving.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
        }

        /// <summary>
        /// Draw the model on the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            string assetName = null;
            switch (State)
            {
                case InteractableState.Discard:
                    // fish is destroyed but awaiting cleanup. Don't draw
                    return;
                case InteractableState.Dead:
                    assetName = _deadAssetName;
                    break;
                case InteractableState.Alive:
                    if (Math.Abs(_currentVelocity.X) > _movementBuffer)
                    {
                        if (CurrentHunger <= _hungerDangerValue) assetName = _moveAnimationStarving.CurrentAnimationFrame(gameTime);
                        else if (CurrentHunger <= _hungerWarningValue) assetName = _moveAnimationHungry.CurrentAnimationFrame(gameTime);
                        else if (_level == 1) assetName = _healthyAssetName2;
                        else assetName = _moveAnimationHealthy.CurrentAnimationFrame(gameTime); ;
                    }
                    else
                    {
                        if (CurrentHunger <= _hungerDangerValue) assetName = _starvingAssetName;
                        else if (CurrentHunger <= _hungerWarningValue) assetName = _hungryAssetName;
                        else if (_level == 1) assetName = _healthyAssetName2;
                        else assetName = _healthyAssetName;
                    }
                    break;
            }

            if (!string.IsNullOrWhiteSpace(assetName))
            {
                SpriteEffects spriteEffects = (_facingLeft) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(assetName), BoundaryBox.Location, null, effects: spriteEffects);
            }
        }

        /// <summary>
        /// Invoked by another entity that has targeted this fish as food
        /// </summary>
        public void Eat()
        {
            State = InteractableState.Discard;
        }

        /// <summary>
        /// If fish is hungry, find nearby food and move to consume it
        /// </summary>
        /// <param name="models">List of  all interactable objects on the field</param>
        /// <returns>Bool indicating whether targeting a source of food</returns>
        protected override bool SearchForFood(List<IInteractable> models)
        {
            if (CurrentHunger > _hungerStartValue)
            {
                return false;
            }

            Pellet nearestPellet = models.Where((model) => model is Pellet)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center, BoundaryBox.Center)).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.BoundaryBox.Center, BoundaryBox.Center);
                if (distance < 30)
                {
                    ConsumeFood(nearestPellet.Eat());
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.BoundaryBox.Center - BoundaryBox.Center);
                MoveTowards(direction);
                return true;
            }
            return false;
        }

        private void ConsumeFood(float nutrition)
        {
            CurrentHunger += nutrition;
            _totalConsumption += nutrition;
            if (_totalConsumption >= _upgrade1HungerThreshold)
            {
                _level = 1;
                _coinValue = Coin.GoldCoinValue;
            }
        }

        private const float _upgrade1HungerThreshold = 70;

        private int _level = 0;

        /// <summary>
        /// Drawable texture showing the fish
        /// </summary>
        private readonly string _healthyAssetName = "Guppy.png";

        private readonly string _healthyAssetName2 = "GuppyLevel2Asset";

        private readonly string _hungryAssetName = "Guppy_Hungry.png";

        private readonly string _starvingAssetName = "Guppy_Starving.png";

        private readonly string _deadAssetName = "Guppy_Dead.png";

        private Animation _moveAnimationHealthy, _moveAnimationHungry, _moveAnimationStarving;

        private readonly List<string> _animationFramesHealthy = new List<string>()
        {
            "Guppy.png",
            "Guppy_move1.png",
            "Guppy.png",
            "Guppy_move2.png",
        };

        private readonly List<string> _animationFramesHungry = new List<string>()
        {
            "Guppy_Hungry.png",
            "guppy_hungry_move1.png",
            "Guppy_Hungry.png",
            "guppy_hungry_move2.png",
        };

        private readonly List<string> _animationFramesStarving = new List<string>()
        {
            "Guppy_Starving.png",
            "guppy_starving_move1.png",
            "Guppy_Starving.png",
            "guppy_starving_move2.png",
        };

        /// <summary>
        /// Movement speed to trigger move animation
        /// </summary>
        private float _movementBuffer = 0.01f;
    }
}
