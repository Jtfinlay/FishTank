//
// Copyright - James Finlay
// 

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FishTank.Models
{
    public class GoldFish : IInteractable
    {
        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        private const float MaxHunger = 100;

        private const float _maxSpeed = 3.0f;

        private Random _random;

        private Texture2D _texture;

        /// <summary>
        /// Expect fish to move every 4 seconds or so.
        /// </summary>
        private readonly float _probabilityToMovePerFrame = 1f / (3.5f * Game1.ExpectedFramesPerSecond);

        public Vector2 Position { get; private set; }

        public InteractableState State { get; private set; }

        public GoldFish(GraphicsDevice graphicsDevice)
        {
            var rect = new Texture2D(graphicsDevice, 30, 30);

            Color[] data = new Color[30 * 30];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Chocolate;
            }
            rect.SetData(data);

            _texture = rect;
            Position = new Vector2(
                graphicsDevice.Viewport.TitleSafeArea.X, 
                graphicsDevice.Viewport.TitleSafeArea.Y + Constants.VirtualHeight / 2);

            _random = new Random();
        }

        public void Update(List<IInteractable> models)
        {

        }

        /// <summary>
        /// Draw the model on the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null);
        }
    }
}
