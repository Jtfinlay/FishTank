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

using FishTank.Content;
using FishTank.Instrumentation;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    /// <summary>
    /// Class instance for the <see cref="GuppyFish"/>. This is the simplest <see cref="Fish"/> available in the game. It
    /// chases food, drops coins, and dies. Upgrades to a <see cref="ClownFish"/>.
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

            _dropCoinTime = TimeSpan.MaxValue;
            _maxHunger = 1.0f;
            _coinValue = 0;
            CurrentHunger = _maxHunger;

            BoundaryBox = new Rectangle2(_swimArea.X + Constants.VirtualWidth / 2, 100, 47, 40);

            _moveAnimationHealthy= new Animation(_animationFramesHealthy);
            _moveAnimationHungry = new Animation(_animationFramesHungry);
            _moveAnimationStarving = new Animation(_animationFramesStarving);

            // Preload assets
            _animationFramesHealthy.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            _animationFramesHungry.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            _animationFramesStarving.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            ContentBuilder.Instance.LoadTextureByName(_deadAsset);
        }

        /// <summary>
        /// Draw the <see cref="GuppyFish"/> to the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        /// <param name="gameTime">Time measurements for the game world</param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            string assetName = string.Empty;

            switch (State)
            {
                case InteractableState.Discard:
                    // fish is destroyed but awaiting cleanup. Don't draw
                    break;
                case InteractableState.Dead:
                    assetName = _deadAsset;
                    break;
                case InteractableState.Alive:
                    bool fishIsMoving = Math.Abs(_currentVelocity.Length()) > _movementBuffer;
                    if (CurrentHunger <= _hungerDangerValue)
                    {
                        assetName = (fishIsMoving) ? _moveAnimationStarving.CurrentAnimationFrame(gameTime) : _starvingAsset;
                    }
                    else if (CurrentHunger <= _hungerWarningValue)
                    {
                        assetName = (fishIsMoving) ? _moveAnimationHungry.CurrentAnimationFrame(gameTime) : _hungryAsset;
                    }
                    else
                    {
                        assetName = (fishIsMoving) ? _moveAnimationHealthy.CurrentAnimationFrame(gameTime) : _healthyAsset;
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
            if (_totalConsumption >= _upgradeHungerThreshold)
            {
                InvokeOnItemDrop(this, new ItemDropEventArgs(new ClownFish(BoundaryBox.Left, BoundaryBox.Top)));
                State = InteractableState.Discard;
            }
        }

        private const float _upgradeHungerThreshold = 1;

        /// <summary>
        /// Constant width used for all instances of the <see cref="GuppyFish"/>.
        /// </summary>
        private const int _width = 70;

        /// <summary>
        /// Constant height used for all instances of the <see cref="GuppyFish"/>.
        /// </summary>
        private const int _height = 60;

        /// <summary>
        /// Threshold value which, when current velocity surpasses, triggers use of move animations
        /// </summary>
        private float _movementBuffer = 0.01f;

        /// <summary>
        /// Set of <see cref="Animation"/> objects used for the different hunger states 
        /// of the <see cref="GuppyFish"/> during movement.
        /// </summary>
        private Animation _moveAnimationHealthy, _moveAnimationHungry, _moveAnimationStarving;

        /// <summary>
        /// Name of asset to use when <see cref="GuppyFish"/>'s hunger is at a healthy level.
        /// </summary>
        private readonly string _healthyAsset = "GuppyFish\\healthy.png";

        /// <summary>
        /// Name of asset to use when <see cref="GuppyFish"/>'s hunger is at a hungry level.
        /// </summary>
        private readonly string _hungryAsset = "GuppyFish\\hungry.png";

        /// <summary>
        /// Name of asset to use when <see cref="GuppyFish"/>'s hunger is at a starving level.
        /// </summary>
        private readonly string _starvingAsset = "GuppyFish\\starving.png";

        /// <summary>
        /// Name of asset to use when <see cref="GuppyFish"/> is dead.
        /// </summary>
        private readonly string _deadAsset = "GuppyFish\\dead.png";

        /// <summary>
        /// List of movement assets to use when the <see cref="GuppyFish"/>'s hunger is at a healthy level.
        /// </summary>
        private readonly List<string> _animationFramesHealthy = new List<string>()
        {
            "GuppyFish\\healthy.png",
            "GuppyFish\\healthy_move1.png",
            "GuppyFish\\healthy.png",
            "GuppyFish\\healthy_move2.png",
        };

        /// <summary>
        /// List of movement assets to use when the <see cref="GuppyFish"/>'s hunger is at a hungry level.
        /// </summary>
        private readonly List<string> _animationFramesHungry = new List<string>()
        {
            "GuppyFish\\hungry.png",
            "GuppyFish\\hungry_move1.png",
            "GuppyFish\\hungry.png",
            "GuppyFish\\hungry_move2.png",
        };

        /// <summary>
        /// List of movement assets to use when the <see cref="GuppyFish"/>'s hunger is at a starving level.
        /// </summary>
        private readonly List<string> _animationFramesStarving = new List<string>()
        {
            "GuppyFish\\starving.png",
            "GuppyFish\\starving_move1.png",
            "GuppyFish\\starving.png",
            "GuppyFish\\starving_move2.png",
        };
    }
}
