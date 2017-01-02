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
    public class ClownFish : EconomicFish
    {

        /// <summary>
        /// Creates a new instance of the <see cref="ClownFish"/> at given coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the top-left corner of the created <see cref="ClownFish"/>.</param>
        /// <param name="y">Y coordinate of the top-left corner of the created <see cref="ClownFish"/>.</param>
        public ClownFish(float x, float y) : base()
        {
            Log.LogVerbose("Creating clown fish");

            // Set basic values
            _dropCoinTime = TimeSpan.FromSeconds(15);
            _maxHunger = 1.0f;
            _coinValue = Coin.SilverCoinValue;
            CurrentHunger = _maxHunger;

            BoundaryBox = new Rectangle2(x, y, _width, _height);

            // Animation objects
            _moveAnimationHealthy = new Animation(_animationFramesHealthy);
            _moveAnimationHungry = new Animation(_animationFramesHungry);
            _moveAnimationStarving = new Animation(_animationFramesStarving);

            // Preload assets
            _animationFramesHealthy.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            _animationFramesHungry.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            _animationFramesStarving.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
            ContentBuilder.Instance.LoadTextureByName(_deadAsset);
        }

        /// <summary>
        /// Draw the <see cref="ClownFish"/> to the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource used for drawing</param>
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
                    bool fishIsMoving = Math.Abs(_currentVelocity.X) > _movementBuffer;
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
                // TODO
            }
        }

        private const float _upgradeHungerThreshold = 200;

        /// <summary>
        /// Constant width used for all instances of the <see cref="ClownFish"/>.
        /// </summary>
        private const int _width = 70;

        /// <summary>
        /// Constant height used for all instances of the <see cref="ClownFish"/>.
        /// </summary>
        private const int _height = 60;

        /// <summary>
        /// Threshold value which, when current velocity surpasses, triggers use of move animations
        /// </summary>
        private float _movementBuffer = 0.01f;

        /// <summary>
        /// Set of <see cref="Animation"/> objects used for the different hunger states 
        /// of the <see cref="ClownFish"/> during movement.
        /// </summary>
        private Animation _moveAnimationHealthy, _moveAnimationHungry, _moveAnimationStarving;

        /// <summary>
        /// Name of asset to use when <see cref="ClownFish"/>'s hunger is at a healthy level.
        /// </summary>
        private readonly string _healthyAsset = "ClownFish\\healthy.png";

        /// <summary>
        /// Name of asset to use when <see cref="ClownFish"/>'s hunger is at a hungry level.
        /// </summary>
        private readonly string _hungryAsset = "ClownFish\\hungry.png";

        /// <summary>
        /// Name of asset to use when <see cref="ClownFish"/>'s hunger is at a starving level.
        /// </summary>
        private readonly string _starvingAsset = "ClownFish\\starving.png";

        /// <summary>
        /// Name of asset to use when <see cref="ClownFish"/> is dead.
        /// </summary>
        private readonly string _deadAsset = "ClownFish\\dead.png";

        /// <summary>
        /// List of movement assets to use when the <see cref="ClownFish"/>'s hunger is at a healthy level.
        /// </summary>
        private readonly List<string> _animationFramesHealthy = new List<string>()
        {
            "ClownFish\\healthy.png",
            "ClownFish\\healthy_move1.png",
            "ClownFish\\healthy.png",
            "ClownFish\\healthy_move2.png",
        };

        /// <summary>
        /// List of movement assets to use when the <see cref="ClownFish"/>'s hunger is at a hungry level.
        /// </summary>
        private readonly List<string> _animationFramesHungry = new List<string>()
        {
            "ClownFish\\hungry.png",
            "ClownFish\\hungry_move1.png",
            "ClownFish\\hungry.png",
            "ClownFish\\hungry_move2.png",
        };

        /// <summary>
        /// List of movement assets to use when the <see cref="ClownFish"/>'s hunger is at a starving level.
        /// </summary>
        private readonly List<string> _animationFramesStarving = new List<string>()
        {
            "ClownFish\\starving.png",
            "ClownFish\\starving_move1.png",
            "ClownFish\\starving.png",
            "ClownFish\\starving_move2.png",
        };
    }
}
