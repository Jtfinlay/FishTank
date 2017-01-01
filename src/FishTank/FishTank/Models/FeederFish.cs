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
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FishTank.Models
{
    /// <summary>
    /// Unique helper fish that swims around dropping pellets for other fish to collect.
    /// </summary>
    public class FeederFish : Fish
    {
        public FeederFish() : base()
        {
            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle2(_swimArea.X + Constants.VirtualWidth / 2, 100, 60, 75);

            // Preload assets
            _moveAnimation = new Animation(_animationFrames);
            ContentBuilder.Instance.LoadTextureByName(_stillAsset);
            _animationFrames.ForEach((frame) => ContentBuilder.Instance.LoadTextureByName(frame));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteEffects spriteEffects = (_facingLeft) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            string assetName = string.Empty;
            if (Math.Abs(_currentVelocity.Length()) > _movementBuffer)
            {
                assetName = _moveAnimation.CurrentAnimationFrame(gameTime);
            }
            else
            {
                assetName = _stillAsset;
            }
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(assetName), BoundaryBox.Location, null, effects: spriteEffects);
        }

        protected override bool SearchForFood(List<IInteractable> models)
        {
            return false;
        }

        protected override void UpdateAlive(List<IInteractable> models, GameTime gameTime)
        {
            _timeSinceFoodDrop += gameTime.ElapsedGameTime;
            if (_timeSinceFoodDrop >= _timeBetweenFoodDrops)
            {
                _timeSinceFoodDrop = TimeSpan.Zero;

                Vector2 launchPosition = new Vector2(BoundaryBox.Center.X, BoundaryBox.Top);
                Vector2 launchVelocity = new Vector2(_pelletLaunchSpeed * (_facingLeft ? -1 : 1), 0);
                InvokeOnItemDrop(this, new ItemDropEventArgs(new Pellet(launchPosition, launchVelocity)));
                return;
            }

            WanderAround();
        }

        private Animation _moveAnimation;

        private string _stillAsset = "seahorse.png";

        private readonly List<string> _animationFrames = new List<string>()
        {
            "seahorse.png",
            "seahorse2.png",
            "seahorse3.png",
            "seahorse2.png",
        };

        /// <summary>
        /// Movement speed to trigger move animation
        /// </summary>
        private float _movementBuffer = 0.05f;

        /// <summary>
        /// Timespan tracking the time since the last food drop
        /// </summary>
        private TimeSpan _timeSinceFoodDrop = TimeSpan.Zero;

        /// <summary>
        /// Timespan for how often the fish drops a food pellet
        /// </summary>
        private readonly TimeSpan _timeBetweenFoodDrops = TimeSpan.FromSeconds(8);

        private const float _pelletLaunchSpeed = 6f;
    }
}
