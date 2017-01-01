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
    /// Abstract base class for all fish types
    /// </summary>
    public abstract class Fish : IInteractable
    {
        public event EventHandler<ItemDropEventArgs> OnItemDrop;

        /// <summary>
        /// State of the goldfish indicating whether interactable with other objects
        /// </summary>
        public InteractableState State { get; protected set; }

        /// <summary>
        /// Instance of <see cref="Rectangle2"/> indicating the position and dimensions of the <see cref="Fish"/>
        /// </summary>
        public Rectangle2 BoundaryBox { get; protected set; }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        /// <summary>
        /// Perform regular actions if the fish is alive
        /// </summary>
        /// <param name="models">List of all interactable objects on the field</param>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <remarks>Movement should go as far as setting velocity. Translation is performed by base class</remarks>
        protected abstract void UpdateAlive(List<IInteractable> models, GameTime gameTime);

        /// <summary>
        /// If fish is hungry, find nearby food and move to consume it
        /// </summary>
        /// <param name="models">List of  all interactable objects on the field</param>
        /// <returns>Bool indicating whether targeting a source of food</returns>
        protected abstract bool SearchForFood(List<IInteractable> models);

        protected void InvokeOnItemDrop(object sender, ItemDropEventArgs eventArgs)
        {
            OnItemDrop?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Search for nearby food, continue ongoing actions, or wander around the tank
        /// </summary>
        /// <param name="models">List of all interactable objects on the field</param>
        public void Update(List<IInteractable> models, GameTime gameTime)
        {
            switch (State)
            {
                case InteractableState.Alive:
                    UpdateAlive(models, gameTime);
                    Translate(_currentVelocity);
                    break;
                case InteractableState.Dead:
                    UpdateDead();
                    break;
                case InteractableState.Discard:
                default:
                    break;
            }

            if (_currentVelocity.X > 0)
            {
                _facingLeft = false;
            }
            else if (_currentVelocity.X < 0)
            {
                _facingLeft = true;
            }
        }

        /// <summary>
        /// Wander around if no other target exists
        /// </summary>
        protected void WanderAround()
        {
            // Continue wandering to target if it is set
            if (_wanderingTarget != null)
            {
                float distance = Vector2.Distance((Vector2)_wanderingTarget, BoundaryBox.Center);
                if (distance < BoundaryBox.Width)
                {
                    _wanderingTarget = null;
                    return;
                }

                Vector2 direction = Vector2.Normalize((Vector2)_wanderingTarget - BoundaryBox.Center);
                MoveTowards(direction);
                return;
            }

            // Perform probability check to see whether to wander
            if (RandomAccessor.Instance.NextDouble() > _chanceToMovePerFrame)
            {
                SlowDown();
                return;
            }

            _wanderingTarget = CreateWanderDestination();
        }

        /// <summary>
        /// Accelerate fish's velocity towards given direction
        /// </summary>
        /// <param name="direction">Direction to move towards</param>
        protected virtual void MoveTowards(Vector2 direction)
        {
            float acceleration = (_wanderingTarget == null) ? _maxAccelerationRate : _maxWanderAccelerationRate;
            float maxSpeed = (_wanderingTarget == null) ? _maxSpeed : _maxWanderSpeed;

            _currentVelocity += acceleration * direction;

            float currentSpeed = _currentVelocity.Length();
            if (currentSpeed > maxSpeed)
            {
                Vector2 velocityDirection = _currentVelocity / _currentVelocity.Length();
                _currentVelocity = velocityDirection * maxSpeed;
            }
        }

        /// <summary>
        /// Slow the fish's movement down until it is at rest
        /// </summary>
        protected void SlowDown()
        {
            if (_currentVelocity.Length() == 0)
            {
                return;
            }

            Vector2 currentDirection = _currentVelocity / _currentVelocity.Length();
            Vector2 targetVelocity = _currentVelocity - _maxAccelerationRate * currentDirection;

            // We want to get to rest. Not switch directions
            if (_currentVelocity.X > 0 && targetVelocity.X < 0) targetVelocity.X = 0;
            if (_currentVelocity.X < 0 && targetVelocity.X > 0) targetVelocity.X = 0;
            if (_currentVelocity.Y > 0 && targetVelocity.Y < 0) targetVelocity.Y = 0;
            if (_currentVelocity.Y < 0 && targetVelocity.Y > 0) targetVelocity.Y = 0;

            _currentVelocity = targetVelocity;
        }

        /// <summary>
        /// Move gold fish towards direction at given speed
        /// </summary>
        /// <param name="direction">Destination vector to target</param>
        /// <param name="speed">Amount to move every frame</param>
        protected void Translate(Vector2 direction, float speed)
        {
            Translate(direction * speed);
        }

        /// <summary>
        /// Moved fish with given velocity
        /// </summary>
        /// <param name="velocity">Direction & magnitude to translate fish position</param>
        protected virtual void Translate(Vector2 velocity)
        {
            Vector2 nextPosition = BoundaryBox.Location + velocity;

            float rightBoundary = _swimArea.Right - BoundaryBox.Width;
            float bottomBoundary = _swimArea.Bottom - BoundaryBox.Height;

            // Ensure position is not out of bounds
            nextPosition.X = (nextPosition.X > 0) ? nextPosition.X : 0;
            nextPosition.X = (nextPosition.X < rightBoundary) ? nextPosition.X : rightBoundary;
            nextPosition.Y = (nextPosition.Y > 0) ? nextPosition.Y : 0;
            nextPosition.Y = (nextPosition.Y < bottomBoundary) ? nextPosition.Y : bottomBoundary;

            // If at edge of boundaries, velocity should be at rest
            _currentVelocity.X = (nextPosition.X == 0 || nextPosition.X == rightBoundary) ? 0 : _currentVelocity.X;
            _currentVelocity.Y = (nextPosition.Y == 0 || nextPosition.Y == bottomBoundary) ? 0 : _currentVelocity.Y;

            BoundaryBox = new Rectangle2(nextPosition, BoundaryBox.Size);
        }

        /// <summary>
        /// Create a destination for this goldfish to wander to
        /// </summary>
        /// <returns>Destination vector</returns>
        private Vector2 CreateWanderDestination()
        {
            var angle = RandomAccessor.Instance.NextDouble() * Math.PI * 2;
            float radius = (float)Math.Sqrt(RandomAccessor.Instance.NextDouble()) * _maxWanderDistance;
            float x = BoundaryBox.Center.X + radius * (float)Math.Cos(angle);
            float y = BoundaryBox.Center.Y + radius * (float)Math.Sin(angle);

            // If the fish is on the wall, encourage it to move away.
            if (BoundaryBox.Left <= 0)
            {
                x = Math.Abs(x);
            }
            if (BoundaryBox.Right >= _swimArea.Width - BoundaryBox.Width)
            {
                x = -1 * Math.Abs(x);
            }
            if (BoundaryBox.Top <= 0)
            {
                y = Math.Abs(y);
            }
            if (BoundaryBox.Bottom >= _swimArea.Height - BoundaryBox.Height)
            {
                y = -1 * Math.Abs(y);
            }

            // Ensure target is within bounds
            x = (x > 0) ? x : 0;
            x = (x < _swimArea.Right - BoundaryBox.Width) ? x : _swimArea.Right - BoundaryBox.Width;
            y = (y > 0) ? y : 0;
            y = (y < _swimArea.Bottom - BoundaryBox.Height) ? y : _swimArea.Bottom - BoundaryBox.Height;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Update the Goldfish in the dead state
        /// </summary>
        private void UpdateDead()
        {
            Translate(new Vector2(0, Constants.FallSpeed));

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                State = InteractableState.Discard;
            }
        }

        /// <summary>
        /// Whether the fish is facing left or right. This is used to 
        /// prevent fish from turning when it gets to rest
        /// </summary>
        protected bool _facingLeft = true;

        /// <summary>
        /// Rectangle holding the size of the game field.
        /// </summary>
        protected Rectangle _swimArea;

        /// <summary>
        /// Maximum speed of the fish.
        /// </summary>
        protected float _maxSpeed = 4.5f;

        /// <summary>
        /// Maximum acceleration of the fish.
        /// </summary>
        protected float _maxAccelerationRate = 0.6f;

        /// <summary>
        /// Current movement velocity of the fish
        /// </summary>
        protected Vector2 _currentVelocity;

        /// <summary>
        /// If fish is wandering, this is set to maintain destination
        /// </summary>
        protected Vector2? _wanderingTarget = null;

        /// <summary>
        /// Slower wandering speed when no important targets around
        /// </summary>
        protected float _maxWanderSpeed = 2f;

        /// <summary>
        /// Slow wandering acceleration when no important targets around
        /// </summary>
        protected float _maxWanderAccelerationRate = .05f;

        /// <summary>
        /// The max distance to wander
        /// </summary>
        private const float _maxWanderDistance = 300;

        /// <summary>
        /// Expect fish to wander around every 3 seconds
        /// </summary>
        private readonly float _chanceToMovePerFrame = 1f / (3f * Constants.ExpectedFramesPerSecond);
    }
}
