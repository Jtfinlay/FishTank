//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishTank.Views
{
    public interface IView : IClickable
    {
        Matrix PostScaleTransform { get; }

        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        void UnloadContent();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
