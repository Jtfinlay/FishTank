//
//  Copyright 2016 James Finlay
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using FishTank.Components;
using FishTank.Content;
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

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            ContentBuilder.Instantiate(graphicsDevice, content);

            _playButton = new ButtonComponent(new Rectangle(Area.Width / 2 - 150, Area.Height / 2 - 75, 300, 150), ContentBuilder.Instance.GetString("Play"));
            _playButton.OnClick += OnPlayButtonClick;

            _playButton.LoadContent(graphicsDevice, content);
        }

        public override void UnloadContent()
        {
            _playButton.UnloadContent();
            _playButton.OnClick -= OnPlayButtonClick;
            ContentBuilder.Instance.UnloadContent();
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

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            if (_playButton.Area.Contains(mouseEvent.Location))
            {
                return _playButton.MouseEvent(mouseEvent);
            }
            return false;
        }

        private void OnPlayButtonClick(object sender, System.EventArgs e)
        {
            Navigate(new NavigationEventArgs(typeof(GameScreen)) { Level = new Level1() });
        }

        private ButtonComponent _playButton;
    }
}
