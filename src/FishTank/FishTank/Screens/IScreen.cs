//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Screens
{
    public delegate void OnNavigateEventHandler(object sender, NavigationEventArgs e);

    public interface IScreen : IClickable
    {
        event OnNavigateEventHandler OnNavigate;

        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        void UnloadContent();

        void Update(GameTime gameTime, MouseState currentMouseState);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix);
    }
}
