//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishTank.Screens
{
    public class ButtonComponent : IComponent
    {
        public Rectangle Area { get; private set; }

        public Matrix PreTransformMatrix { get; private set; }

        public ButtonComponent()
        {

        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
        }

        public void UnloadContent()
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void MouseEvent(MouseEvent mouseEvent)
        {
        }
    }
}
