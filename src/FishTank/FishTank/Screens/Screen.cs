//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishTank.Screens
{
    public abstract class Screen : Component
    {
        public new Rectangle Area { get; protected set; } = Constants.VirtualArea;

        public event EventHandler<NavigationEventArgs> OnNavigate;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewMatrix);

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, new Matrix());
        }

        protected void Navigate(NavigationEventArgs eventArgs)
        {
            OnNavigate?.Invoke(this, eventArgs);
        }
    }
}
