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

using FishTank.Drawing;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Models
{
    public class Pellet : IInteractable
    {
        public readonly string TextureName = "pellet.png";

        public InteractableState State { get; private set; }

        public Rectangle2 BoundaryBox { get; private set; }

        public Pellet(Vector2 position, Vector2 velocity)
        {
            BoundaryBox = new Rectangle2(position, new Vector2(20, 20));
            _velocity = velocity;
            _velocity.Y = Constants.FallSpeed;
            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);

            ContentBuilder.Instance.LoadTextureByName(TextureName);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (State == InteractableState.Discard)
            {
                // pellet is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(TextureName), BoundaryBox.Location, null);
        }

        public void Update(List<IInteractable> models, GameTime gameTime)
        {
            var targetVelocity = _velocity + (_velocity.X > 0 ? -1 : 1) * _acceleration;
            if (targetVelocity.X < 0 && _velocity.X >= 0) targetVelocity.X = 0;
            if (targetVelocity.X > 0 && _velocity.X <= 0) targetVelocity.X = 0;

            _velocity = targetVelocity;

            var nextPosition = Vector2.Add(BoundaryBox.Location, _velocity);

            float rightBoundary = _swimArea.Right - BoundaryBox.Width;
            float bottomBoundary = _swimArea.Bottom - BoundaryBox.Height;

            // Ensure position is not out of bounds
            nextPosition.X = (nextPosition.X > 0) ? nextPosition.X : 0;
            nextPosition.X = (nextPosition.X < rightBoundary) ? nextPosition.X : rightBoundary;
            nextPosition.Y = (nextPosition.Y > 0) ? nextPosition.Y : 0;
            nextPosition.Y = (nextPosition.Y < bottomBoundary) ? nextPosition.Y : bottomBoundary;

            // If at edge of boundaries, velocity should be at rest
            _velocity.X = (nextPosition.X == 0 || nextPosition.X == rightBoundary) ? 0 : targetVelocity.X;
            _velocity.Y = (nextPosition.Y == 0 || nextPosition.Y == bottomBoundary) ? 0 : targetVelocity.Y;

            BoundaryBox = new Rectangle2(nextPosition, BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                this.State = InteractableState.Discard;
            }
        }

        /// <summary>
        /// Consume the pellet, returning the nutrition value of the food
        /// </summary>
        /// <returns>Nutrition value fo the food</returns>
        public float Eat()
        {
            State = InteractableState.Discard;
            return 1.0f;
        }

        private Rectangle _swimArea;

        private Vector2 _velocity;

        private Vector2 _acceleration = new Vector2(0.1f, 0);
    }
}
