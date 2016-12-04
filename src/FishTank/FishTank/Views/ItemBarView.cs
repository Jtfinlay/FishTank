//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FishTank.Views
{
    public class ItemBarView : IView
    {
        public int Height => Constants.VirtualBarHeight;

        public int Width => Constants.VirtualWidth;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            int padding = 10;
            _buttons = new List<TopbarItem>()
            {
                new TopbarItem(graphicsDevice, Height, padding)
            };

            var rect = new Texture2D(graphicsDevice, Width, Height);

            Color[] data = new Color[Width * Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Coral;
            }
            rect.SetData(data);
            _texture = rect;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime, MouseState virtualMouseState)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(0, 0), null);
            _buttons.ForEach((button) => button.Draw(spriteBatch));
        }

        private Texture2D _texture;

        private List<TopbarItem> _buttons;
    }
}
