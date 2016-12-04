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
        public Vector2 Position { get; private set; }

        public InteractableState State { get; private set; }

        public GoldFish(GraphicsDevice graphicsDevice)
        {
            _random = new Random();
            var rect = new Texture2D(graphicsDevice, 30, 30);

            Color[] data = new Color[30 * 30];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Chocolate;
            }
            rect.SetData(data);
            _texture = rect;

            _safeArea = graphicsDevice.Viewport.TitleSafeArea;

            Position = new Vector2(_safeArea.X + Constants.VirtualWidth / 2, 0);
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
                float distance = Vector2.Distance((Vector2)_movementTarget, Position);
                if (distance < 30)
                {
                    _movementTarget = null;
                    return;
                }

                Vector2 direction = Vector2.Normalize((Vector2)_movementTarget - Position);
                Translate(direction, _wanderSpeed);
                return;
            }

            // Perform probability check to see whether to wander
            if (_random.NextDouble() > _chanceToMovePerFrame)
            {
                return;
            }

            var angle = _random.NextDouble() * Math.PI * 2;
            float radius = (float)Math.Sqrt(_random.NextDouble()) * _wanderDistance;
            float x = Position.X + radius * (float)Math.Cos(angle);
            float y = Position.Y + radius * (float)Math.Sin(angle);
            _movementTarget = new Vector2(x, y);
            return;
        }

        /// <summary>
        /// Draw the model on the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null);
        }

        private void Translate(Vector2 direction, float speed)
        {
            Vector2 nextPosition = Position + direction * speed;
            nextPosition.X = (nextPosition.X > 0) ? nextPosition.X : 0;
            nextPosition.X = (nextPosition.X < _safeArea.Right) ? nextPosition.X : _safeArea.Right;
            nextPosition.Y = (nextPosition.Y > 0) ? nextPosition.Y : 0;
            nextPosition.Y = (nextPosition.Y < _safeArea.Bottom) ? nextPosition.Y : _safeArea.Bottom;
            Position = nextPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns>Bool indicating whether going after a source of food</returns>
        private bool SearchForFood(List<IInteractable> models)
        {
            Pellet nearestPellet = models.Where((model) => model is Pellet)?.OrderBy(i => Vector2.Distance(i.Position, Position)).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.Position, Position);
                if (distance < 30)
                {
                    nearestPellet.Kill();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.Position - Position);
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
