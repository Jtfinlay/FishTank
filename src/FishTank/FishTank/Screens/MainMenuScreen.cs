//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Models.Levels;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Screens
{
    public class MainMenuScreen : Screen
    {
        public MainMenuScreen()
        {
            _playButton = new ButtonComponent(new Rectangle(Area.Width/2-150, Area.Height/2-75, 300, 150), "Play");
            _playButton.OnClick += OnPlayButtonClick;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _playButton.LoadContent(graphicsDevice, content);
        }

        public override void UnloadContent()
        {
            _playButton.UnloadContent();
            _playButton.OnClick -= OnPlayButtonClick;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix)
        {
            spriteBatch.Begin(
                samplerState: SamplerState.LinearClamp,
                blendState: BlendState.AlphaBlend,
                transformMatrix: transformMatrix);

            _playButton.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            _playButton.Update(gameTime, currentMouseState);
        }

        public override void MouseEvent(MouseEvent mouseEvent)
        {
            if (_playButton.Area.Contains(mouseEvent.Location))
            {
                _playButton.MouseEvent(mouseEvent);
            }
        }

        private void OnPlayButtonClick(object sender, System.EventArgs e)
        {
            Navigate(new NavigationEventArgs(typeof(GameScreen)) { Level = new Level1() });
        }

        private ButtonComponent _playButton;
    }
}
