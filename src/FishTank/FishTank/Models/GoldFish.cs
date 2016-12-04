//
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
        public Rectangle Area { get; private set; }

        public InteractableState State { get; private set; }

        public GoldFish(GraphicsDevice graphicsDevice)
        {
            _random = new Random();
            _safeArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            Area = new Rectangle(_safeArea.X + Constants.VirtualWidth / 2, 0, 30, 30);

            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);
            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Chocolate;
            }
            rect.SetData(data);
            _texture = rect;

        }

        public void Update(List<IInteractable> models)
        {
            // First, try to find food if nearby
            if (SearchForFood(models))
            {
                _movementTarget = null;
                return;
            }

            // Continue wandering to target if it is set
            if (_movementTarget != null)
            {
                float distance = Vector2.Distance((Vector2)_movementTarget, Area.Center.ToVector2());
                if (distance < Area.Width)
                {
                    _movementTarget = null;
                    return;
                }

                Vector2 direction = Vector2.Normalize((Vector2)_movementTarget - Area.Center.ToVector2());
                Translate(direction, _wanderSpeed);
                return;
            }

            // Perform probability check to see whether to wander
            if (_random.NextDouble() > _chanceToMovePerFrame)
            {
                return;
            }

            
            _movementTarget = CreateWanderTarget();
            return;
        }

        /// <summary>
        /// Draw the model on the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area.Location.ToVector2(), null);
        }

        private Vector2 CreateWanderTarget()
        {
            var angle = _random.NextDouble() * Math.PI * 2;
            float radius = (float)Math.Sqrt(_random.NextDouble()) * _wanderDistance;
            float x = Area.Center.X + radius * (float)Math.Cos(angle);
            float y = Area.Center.Y + radius * (float)Math.Sin(angle);

            // If the fish is on the wall, encourage it to move away.
            if (Area.Left <= 0)
            {
                x = Math.Abs(x);
            }
            if (Area.Right >= _safeArea.Width - Area.Width)
            {
                x = -1 * Math.Abs(x);
            }
            if (Area.Top <= 0)
            {
                y = Math.Abs(y);
            }
            if (Area.Bottom >= _safeArea.Height - Area.Height)
            {
                y = -1 * Math.Abs(y);
            }

            // Ensure target is within bounds
            x = (x > 0) ? x : 0;
            x = (x < _safeArea.Right - Area.Width) ? x : _safeArea.Right - Area.Width;
            y = (y > 0) ? y : 0;
            y = (y < _safeArea.Bottom - Area.Height) ? y : _safeArea.Bottom - Area.Height;

            return new Vector2(x, y);
        }

        private void Translate(Vector2 direction, float speed)
        {
            Vector2 nextPosition = Area.Location.ToVector2() + direction * speed;
            nextPosition.X = (nextPosition.X > 0) ? nextPosition.X : 0;
            nextPosition.X = (nextPosition.X < _safeArea.Right - Area.Width) ? nextPosition.X : _safeArea.Right - Area.Width;
            nextPosition.Y = (nextPosition.Y > 0) ? nextPosition.Y : 0;
            nextPosition.Y = (nextPosition.Y < _safeArea.Bottom - Area.Height) ? nextPosition.Y : _safeArea.Bottom - Area.Height;
            Area = new Rectangle(nextPosition.ToPoint(), Area.Size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns>Bool indicating whether going after a source of food</returns>
        private bool SearchForFood(List<IInteractable> models)
        {
            Pellet nearestPellet = models.Where((model) => model is Pellet)?
                .OrderBy(i => Vector2.Distance(i.Area.Center.ToVector2(), Area.Center.ToVector2())).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.Area.Center.ToVector2(), Area.Center.ToVector2());
                if (distance < 30)
                {
                    nearestPellet.Kill();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.Area.Center.ToVector2() - Area.Center.ToVector2());
                Translate(direction, _maxSpeed);
                return true;
            }
            return false;
        }

        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        private const float MaxHunger = 100;

        private const float _maxSpeed = 3.0f;

        private const float _wanderSpeed = 1.5f;

        private Vector2? _movementTarget = null;

        private const float _wanderDistance = 300;

        private Rectangle _safeArea;

        private Texture2D _texture;

        private Random _random;

        /// <summary>
        /// Expect fish to wander around every 3 seconds
        /// </summary>
        private readonly float _chanceToMovePerFrame = 1f / (3f * Constants.ExpectedFramesPerSecond);
    }
}
