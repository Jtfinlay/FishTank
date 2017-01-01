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

using FishTank.Components;
using FishTank.Content;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            _maxSpeed = 2.0f;
            _maxAccelerationRate = 0.7f;

            int height = 75;
            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle(_swimArea.X + Constants.VirtualWidth / 2, Constants.VirtualHeight - height, 105, height);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_assetName);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), BoundaryBox.Location.ToVector2(), null);
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

        private string _assetName = "crab.png";
    }
}
