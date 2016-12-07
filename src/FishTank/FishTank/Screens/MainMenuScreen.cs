//
// Copyright - James Finlay
// 

using System;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Screens
{
    public class MainMenuScreen : IScreen
    {
        public Rectangle Area => Constants.VirtualArea;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix)
        {
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void MouseEvent(MouseEvent mouseEvent)
        {
            throw new NotImplementedException();
        }
    }
}
