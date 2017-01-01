﻿//
//  Copyright 2016 James Finlay
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
    /// Helper crab that moves around the bottom of the tank, catching falling coins.
    /// </summary>
    public class CoinCrab : Fish
    {
        public CoinCrab(): base()
        {
            _maxWanderSpeed = 1.5f;
            _maxWanderAccelerationRate = 0.8f;
            _maxSpeed = 3.0f;
            _maxAccelerationRate = 0.9f;

            int height = 75;
            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle(_swimArea.X + Constants.VirtualWidth / 2, Constants.VirtualHeight - height, 105, height);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_stillAsset);
            _animationFrames.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            string assetName = _stillAsset;
            if (Math.Abs(_currentVelocity.X) > _movementBuffer)
            {
                if (gameTime.TotalGameTime > _changeAnimationFrameTarget)
                {
                    _changeAnimationFrameTarget = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_animationDuration);
                    _currentAnimationFrame++;
                    if (_currentAnimationFrame >= _animationFrames.Count)
                    {
                        _currentAnimationFrame = 0;
                    }
                }

                assetName = _animationFrames.ElementAt(_currentAnimationFrame);
            }
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(assetName), BoundaryBox.Location.ToVector2(), null);
        }

        protected override bool SearchForFood(List<IInteractable> models)
        {
            Coin nearestCoin = models.Where((model) => model is Coin)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2())).FirstOrDefault() as Coin;
            if (nearestCoin != null)
            {
                float distance = Vector2.Distance(nearestCoin.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2());
                if (distance < 30)
                {
                    nearestCoin.Gather();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestCoin.BoundaryBox.Center.ToVector2() - BoundaryBox.Center.ToVector2());
                MoveTowards(direction);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Move crab with velocity. Overrides base to remove Y movement.
        /// </summary>
        /// <param name="velocity">Direction & magnitude to translate fish position</param>
        protected override void Translate(Vector2 velocity)
        {
            velocity.Y = 0;
            base.Translate(velocity);
        }

        protected override void UpdateAlive(List<IInteractable> models, GameTime gameTime)
        {
            // First, try to find coins if nearby
            if (SearchForFood(models))
            {
                _wanderingTarget = null;
                return;
            }

            WanderAround();
        }

        private readonly string _stillAsset = "crab.png";

        /// <summary>
        /// Time when to change animation frame.
        /// </summary>
        private TimeSpan _changeAnimationFrameTarget = default(TimeSpan);

        /// <summary>
        /// Time, in milliseconds, until moving to next animation frame.
        /// </summary>
        private int _animationDuration = 100;

        /// <summary>
        /// Index of the next animation frame to draw
        /// </summary>
        private int _currentAnimationFrame = 0;

        /// <summary>
        /// Movement speed to trigger move animation
        /// </summary>
        private float _movementBuffer = 0.3f;

        private readonly List<string> _animationFrames = new List<string>()
        {
            "crab_move1.png",
            "crab.png",
            "crab_move2.png",
            "crab.png",
        };

    }
}
