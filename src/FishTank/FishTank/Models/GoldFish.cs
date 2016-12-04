//
// Copyright - James Finlay
// 

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    public class GoldFish : IInteractable
    {
        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        private const float MaxHunger = 100;

        private const float _maxSpeed = 3.0f;

        private Texture2D _texture;

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
                graphicsDevice.Viewport.TitleSafeArea.X + Constants.VirtualWidth / 2, 
                0);
        }

        public void Update(List<IInteractable> models)
        {
            Pellet nearestPellet = models.Where((model) => model is Pellet)?.OrderBy(i => Vector2.Distance(i.Position, Position)).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.Position, Position);
                if (distance < 30)
                {
                    nearestPellet.Kill();
                    return;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.Position - Position);
                Position += direction * _maxSpeed;
            }
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
