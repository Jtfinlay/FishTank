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

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FishTank.Models
{
    public abstract class Fish : IInteractable
    {
        public event EventHandler OnCoinDrop;

        /// <summary>
        /// Rectangle indicating the boundary box for the gold fish
        /// </summary>
        public Rectangle BoundaryBox { get; protected set; }

        /// <summary>
        /// State of the goldfish indicating whether interactable with other objects
        /// </summary>
        public InteractableState State { get; protected set; }

        public Fish()
        {
            _random = new Random();
        }

        public abstract void Draw(SpriteBatch spriteBatch);

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
                    break;
                case InteractableState.Dead:
                    UpdateDead();
                    break;
                case InteractableState.Discard:
                default:
                    break;
            }
        }

        protected virtual void UpdateAlive(List<IInteractable> models, GameTime gameTime)
        {
            // Check whether to drop coin
            _timeSinceCoinDrop += gameTime.ElapsedGameTime;
            if (_timeSinceCoinDrop >= _dropCoinTime)
            {
                _timeSinceCoinDrop = TimeSpan.Zero;
                OnCoinDrop?.Invoke(this, null);
            }

            // First, try to find food if nearby
            if (SearchForFood(models))
            {
                _wanderingTarget = null;
                return;
            }

            _currentHunger -= _hungerDropPerFrame;
            if (_currentHunger <= 0)
            {
                State = InteractableState.Dead;
                return;
            }

            // Continue wandering to target if it is set
            if (_wanderingTarget != null)
            {
                float distance = Vector2.Distance((Vector2)_wanderingTarget, BoundaryBox.Center.ToVector2());
                if (distance < BoundaryBox.Width)
                {
                    _wanderingTarget = null;
                    return;
                }

                Vector2 direction = Vector2.Normalize((Vector2)_wanderingTarget - BoundaryBox.Center.ToVector2());
                Translate(direction, _wanderSpeed);
                return;
            }

            // Perform probability check to see whether to wander
            if (_random.NextDouble() > _chanceToMovePerFrame)
            {
                return;
            }

            _wanderingTarget = CreateWanderDestination();
            return;
        }

        /// <summary>
        /// If fish is hungry, find nearby food and move to consume it
        /// </summary>
        /// <param name="models">List of  all interactable objects on the field</param>
        /// <returns>Bool indicating whether targeting a source of food</returns>
        protected abstract bool SearchForFood(List<IInteractable> models);

        /// <summary>
        /// Move gold fish towards the given target at given speed
        /// </summary>
        /// <param name="direction">Destination vector to target</param>
        /// <param name="speed">Amount to move every frame</param>
        protected void Translate(Vector2 direction, float speed)
        {
            Vector2 nextPosition = BoundaryBox.Location.ToVector2() + direction * speed;
            nextPosition.X = (nextPosition.X > 0) ? nextPosition.X : 0;
            nextPosition.X = (nextPosition.X < _swimArea.Right - BoundaryBox.Width) ? nextPosition.X : _swimArea.Right - BoundaryBox.Width;
            nextPosition.Y = (nextPosition.Y > 0) ? nextPosition.Y : 0;
            nextPosition.Y = (nextPosition.Y < _swimArea.Bottom - BoundaryBox.Height) ? nextPosition.Y : _swimArea.Bottom - BoundaryBox.Height;
            BoundaryBox = new Rectangle(nextPosition.ToPoint(), BoundaryBox.Size);
        }

        /// <summary>
        /// Update the Goldfish in the dead state
        /// </summary>
        private void UpdateDead()
        {
            var position = Vector2.Add(BoundaryBox.Location.ToVector2(), new Vector2(0, Constants.FallSpeed));
            BoundaryBox = new Rectangle(position.ToPoint(), BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                State = InteractableState.Discard;
            }
        }

        /// <summary>
        /// Create a destination for this goldfish to wander to
        /// </summary>
        /// <returns>Destination vector</returns>
        private Vector2 CreateWanderDestination()
        {
            var angle = _random.NextDouble() * Math.PI * 2;
            float radius = (float)Math.Sqrt(_random.NextDouble()) * _wanderDistance;
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
        /// Rectangle holding the size of the game field.
        /// </summary>
        protected Rectangle _swimArea;

        /// <summary>
        /// The amount current hunger decriments per frame
        /// </summary>
        private const float _hungerDropPerFrame = 1 / (Constants.ExpectedFramesPerSecond * 25);

        /// <summary>
        /// Current hunger of the fish. At zero the fish dies
        /// </summary>
        protected float _currentHunger;

        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        protected float _maxHunger;

        /// <summary>
        /// Value where fish hits first level of hunger.
        /// </summary>
        protected const float _hungerStartsValue = .7f;
        
        /// <summary>
        /// Value where fish hits 'warning' level of hunger.
        /// </summary>
        protected const float _hungerWarningValue = .5f;

        /// <summary>
        /// Value where fish hits 'danger' elvel of hunger
        /// </summary>
        protected const float _hungerDangerValue = .2f;

        /// <summary>
        /// Timespan indicating how often the fish should drop a coin. Required.
        /// </summary>
        protected TimeSpan _dropCoinTime;

        /// <summary>
        /// maximum speed of the gold fish. Used when targeting food. Required.
        /// </summary>
        protected float _maxSpeed;

        /// <summary>
        /// Timespan tracking the time since the last coin drop
        /// </summary>
        protected TimeSpan _timeSinceCoinDrop = TimeSpan.Zero;

        /// <summary>
        /// If fish is wandering, this is set to maintain destination
        /// </summary>
        private Vector2? _wanderingTarget = null;

        /// <summary>
        /// Slow wandering speed of the fish when no important targets around.
        /// </summary>
        private const float _wanderSpeed = 2f;

        /// <summary>
        /// Distance to wander 
        /// </summary>
        private const float _wanderDistance = 300;

        /// <summary>
        /// Random object used for probability checks
        /// </summary>
        private Random _random;

        /// <summary>
        /// Expect fish to wander around every 3 seconds
        /// </summary>
        private readonly float _chanceToMovePerFrame = 1f / (3f * Constants.ExpectedFramesPerSecond);

    }
}
