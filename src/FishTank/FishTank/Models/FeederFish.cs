//
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
            BoundaryBox = new Rectangle(_swimArea.X + Constants.VirtualWidth / 2, 100, 60, 75);

            // Preload assets
            ContentBuilder.Instance.CreateRectangleTexture(_assetName, BoundaryBox.Width, BoundaryBox.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), BoundaryBox.Location.ToVector2(), Color.AliceBlue);
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
                InvokeOnItemDrop(this, new ItemDropEventArgs(typeof(Pellet), launchPosition, launchVelocity));
                return;
            }

            WanderAround();
        }

        private string _assetName = "FeederFish";

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
