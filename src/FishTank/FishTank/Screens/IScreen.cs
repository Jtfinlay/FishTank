//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Screens
{
    interface IScreen
    {
        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        void UnloadContent();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix);

        void MouseHover(MouseState mouseState);

        void MouseClick(MouseState mouseState);

        void MouseRelease(MouseState mouseState);
    }
}
