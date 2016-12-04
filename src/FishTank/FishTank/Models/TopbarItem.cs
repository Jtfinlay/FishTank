//
// Copyright - James Finlay
// 

using FishTank.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Models
{
    public class TopbarItem : IClickable
    {
        public string Name { get; private set; }

        public Rectangle Area { get; private set; }

        public event EventHandler OnClicked;

        public TopbarItem(GraphicsDevice graphicsDevice, Rectangle area)
        {
            Area = area;

            var rect = new Texture2D(graphicsDevice, Area.Width - _padding*2, Area.Height - _padding*2);

            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Red;
            }
            rect.SetData(data);
            _texture = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area.Location.ToVector2() + new Vector2(_padding,_padding), null);
        }

        public void MouseHover(MouseState mouseState)
        {
        }

        public void MouseClick(MouseState mouseState)
        {
            OnClicked?.Invoke(this, new EventArgs());
        }

        public void MouseRelease(MouseState mouseState)
        {
        }

        private Texture2D _texture;

        private const int _padding = 10;
    }
}
