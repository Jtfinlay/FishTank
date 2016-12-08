//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Screens
{
    public class MainMenuScreen : IScreen
    {
        public event OnNavigateEventHandler OnNavigate;

        public Rectangle Area => Constants.VirtualArea;

        public MainMenuScreen()
        {
            _playButton = new ButtonComponent(new Rectangle(Area.Width/2-150, Area.Height/2-75, 300, 150), "Play");
            _playButton.OnClick += _playButton_OnClick;
        }

        private void _playButton_OnClick(object sender, System.EventArgs e)
        {
            OnNavigate?.Invoke(this, new NavigationEventArgs(typeof(GameScreen)));
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _playButton.LoadContent(graphicsDevice, content);
        }

        public void UnloadContent()
        {
            _playButton.UnloadContent();
            _playButton.OnClick -= _playButton_OnClick;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix)
        {
            spriteBatch.Begin(
                samplerState: SamplerState.LinearClamp,
                blendState: BlendState.AlphaBlend,
                transformMatrix: transformMatrix);

            _playButton.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, MouseState currentMouseState)
        {
            _playButton.Update(gameTime, currentMouseState);
        }

        public void MouseEvent(MouseEvent mouseEvent)
        {
            if (_playButton.Area.Contains(mouseEvent.Location))
            {
                _playButton.MouseEvent(mouseEvent);
            }
        }

        private ButtonComponent _playButton;
    }
}
