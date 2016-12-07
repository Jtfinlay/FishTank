//
// Copyright - James Finlay
// 

using FishTank.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using FishTank.Utilities;

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

        public void MouseEvent(MouseEvent mouseEvent)
        {
            switch (mouseEvent.Action)
            {
                case MouseAction.Click:
                    OnClicked?.Invoke(this, new EventArgs());
                    break;
                case MouseAction.Hover:
                case MouseAction.HoverExit:
                case MouseAction.Release:
                default:
                    break;
            }
        }

        private Texture2D _texture;

        private const int _padding = 10;
    }
}
