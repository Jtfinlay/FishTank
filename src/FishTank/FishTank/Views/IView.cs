//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Views
{
    public interface IView
    {
        Matrix PostScaleTransform { get; }

        int Width { get; }

        int Height { get; }

        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        void UnloadContent();

        void Update(GameTime gameTime, MouseState virtualMouseState);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
