﻿//
// Copyright - James Finlay
// 

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    public class GoldFish : IInteractable
    {
        /// <summary>
        /// Rectangel indicating hte boundary box for the gold fish
        /// </summary>
        public Rectangle BoundaryBox { get; private set; }

        /// <summary>
        /// State of the goldfish indicating whether interactable with other objects
        /// </summary>
        public InteractableState State { get; private set; }

        /// <summary>
        /// Basic fish that chases food and dies
        /// </summary>
        /// <param name="graphicsDevice">Graphics resource for texture creation</param>
        public GoldFish(GraphicsDevice graphicsDevice)
        {
            _random = new Random();
            _safeArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle(_safeArea.X + Constants.VirtualWidth / 2, 0, 30, 30);

            var rect = new Texture2D(graphicsDevice, BoundaryBox.Width, BoundaryBox.Height);
            Color[] data = new Color[BoundaryBox.Width * BoundaryBox.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Chocolate;
            }
            rect.SetData(data);
            _texture = rect;

        }

        /// <summary>
        /// Search for nearby food, continue ongoing actions, or wander around the tank
        /// </summary>
        /// <param name="models">List of all interactable objects on the field</param>
        public void Update(List<IInteractable> models)
        {
            // First, try to find food if nearby
            if (SearchForFood(models))
            {
                _wanderingTarget = null;
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
        /// Draw the model on the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, BoundaryBox.Location.ToVector2(), null);
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
            if (BoundaryBox.Right >= _safeArea.Width - BoundaryBox.Width)
            {
                x = -1 * Math.Abs(x);
            }
            if (BoundaryBox.Top <= 0)
            {
                y = Math.Abs(y);
            }
            if (BoundaryBox.Bottom >= _safeArea.Height - BoundaryBox.Height)
            {
                y = -1 * Math.Abs(y);
            }

            // Ensure target is within bounds
            x = (x > 0) ? x : 0;
            x = (x < _safeArea.Right - BoundaryBox.Width) ? x : _safeArea.Right - BoundaryBox.Width;
            y = (y > 0) ? y : 0;
            y = (y < _safeArea.Bottom - BoundaryBox.Height) ? y : _safeArea.Bottom - BoundaryBox.Height;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Move gold fish towards the given target at given speed
        /// </summary>
        /// <param name="direction">Destination vector to target</param>
        /// <param name="speed">Amount to move every frame</param>
        private void Translate(Vector2 direction, float speed)
        {
            Vector2 nextPosition = BoundaryBox.Location.ToVector2() + direction * speed;
            nextPosition.X = (nextPosition.X > 0) ? nextPosition.X : 0;
            nextPosition.X = (nextPosition.X < _safeArea.Right - BoundaryBox.Width) ? nextPosition.X : _safeArea.Right - BoundaryBox.Width;
            nextPosition.Y = (nextPosition.Y > 0) ? nextPosition.Y : 0;
            nextPosition.Y = (nextPosition.Y < _safeArea.Bottom - BoundaryBox.Height) ? nextPosition.Y : _safeArea.Bottom - BoundaryBox.Height;
            BoundaryBox = new Rectangle(nextPosition.ToPoint(), BoundaryBox.Size);
        }

        /// <summary>
        /// Target available food on the field and move to consume it
        /// </summary>
        /// <param name="models">List of  all interactable objects on the field</param>
        /// <returns>Bool indicating whether going after a source of food</returns>
        private bool SearchForFood(List<IInteractable> models)
        {
            Pellet nearestPellet = models.Where((model) => model is Pellet)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2())).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2());
                if (distance < 30)
                {
                    nearestPellet.Kill();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.BoundaryBox.Center.ToVector2() - BoundaryBox.Center.ToVector2());
                Translate(direction, _maxSpeed);
                return true;
            }
            return false;
        }

        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        private const float MaxHunger = 100;


        /// <summary>
        /// Maximum speed of the gold fish. Used when targeting food or running from aliens
        /// </summary>
        private const float _maxSpeed = 3.0f;

        /// <summary>
        /// Slow wandering speed of the fish when no important targets around.
        /// </summary>
        private const float _wanderSpeed = 1.5f;

        /// <summary>
        /// If fish is wandering, this is set to maintain destination
        /// </summary>
        private Vector2? _wanderingTarget = null;

        /// <summary>
        /// Distance to wander 
        /// </summary>
        private const float _wanderDistance = 300;

        /// <summary>
        /// Rectangle holding the size of the game field.
        /// </summary>
        private Rectangle _safeArea;

        /// <summary>
        /// Drawable texture showing the fish
        /// </summary>
        private Texture2D _texture;

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
