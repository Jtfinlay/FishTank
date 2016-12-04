//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishTank.Models
{
    public class TopbarItem
    {
        public string Name { get; private set; }

        public event EventHandler OnClicked;

        public TopbarItem(GraphicsDevice graphicsDevice, int parentSize, int padding)
        {
            var size = parentSize - padding * 2;
            _padding = padding;

            var rect = new Texture2D(graphicsDevice, size, size);

            Color[] data = new Color[size * size];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Red;
            }
            rect.SetData(data);
            _texture = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(_padding, _padding), null);
        }

        private Texture2D _texture;
        private int _padding;
    }
}
