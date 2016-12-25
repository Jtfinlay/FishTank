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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Models
{
    public class Pellet : IInteractable
    {
        public readonly string TextureName = "PelletRectangleAsset";

        public InteractableState State { get; private set; }

        public Rectangle BoundaryBox { get; private set; }

        public Pellet(Vector2 position, Vector2 velocity)
        {
            BoundaryBox = new Rectangle(position.ToPoint(), new Point(20, 20));
            ContentBuilder.Instance.CreateRectangleTexture(TextureName, BoundaryBox.Width, BoundaryBox.Height);
            _velocity = velocity;
            _velocity.Y = Constants.FallSpeed;
            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == InteractableState.Discard)
            {
                // pellet is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(TextureName), BoundaryBox.Location.ToVector2(), Color.LightGreen);
        }

        public void Update(List<IInteractable> models, GameTime gameTime)
        {
            var targetVelocity = _velocity + (_velocity.X > 0 ? -1 : 1) * _acceleration;
            if (targetVelocity.X < 0 && _velocity.X >= 0) targetVelocity.X = 0;
            if (targetVelocity.X > 0 && _velocity.X <= 0) targetVelocity.X = 0;

            _velocity = targetVelocity;

            var nextPosition = Vector2.Add(BoundaryBox.Location.ToVector2(), _velocity);

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

            BoundaryBox = new Rectangle(nextPosition.ToPoint(), BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                this.State = InteractableState.Discard;
            }
        }

        public void Eat()
        {
            State = InteractableState.Discard;
        }

        private Rectangle _swimArea;

        private Vector2 _velocity;

        private Vector2 _acceleration = new Vector2(0.1f, 0);
    }
}
