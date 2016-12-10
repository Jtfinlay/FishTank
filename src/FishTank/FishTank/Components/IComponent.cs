//
// Copyright - James Finlay
// 

using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Components
{
    public abstract class Component : IClickable
    {
        public Rectangle Area { get; protected set; }

        public abstract bool MouseEvent(MouseEvent mouseEvent);

        public abstract void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        public abstract void UnloadContent();

        public abstract void Update(GameTime gameTime, MouseState currentMouseState);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
