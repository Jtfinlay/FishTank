//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Components
{
    public interface IComponent : IClickable
    {
        Matrix PreTransformMatrix { get; }

        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        void UnloadContent();

        void Update(GameTime gameTime, MouseState currentMouseState);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
