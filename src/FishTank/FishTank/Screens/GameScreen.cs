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
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishTank.Screens
{
    public class GameScreen : Screen
    {

        public GameScreen(NavigationEventArgs e)
        {
            if (e?.Level == null)
            {
                throw new ArgumentNullException($"{nameof(e)}'s level cannot be null.");
            }

            _topBarView = new ItemBarComponent(e.Level);
            _tankView = new TankComponent(0, _topBarView.Area.Height);
            _tankView.OnCoinClick += _tankView_OnCoinClick;

            _topBarView.OnPurchaseFish += PurchaseGoldFish;
        }

        private void _tankView_OnCoinClick(object sender, EventArgs e)
        {
            Coin coin = sender as Coin;
            _topBarView.GoldAmount += coin.GoldValue;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            ContentBuilder.Instantiate(graphicsDevice, content);
            _tankView.LoadContent(graphicsDevice, content);
            _topBarView.LoadContent(graphicsDevice, content);

            _tankView.AddGoldFish();
        }

        public override void UnloadContent()
        {
            _topBarView.OnPurchaseFish -= PurchaseGoldFish;

            _tankView.UnloadContent();
            _topBarView.UnloadContent();
            ContentBuilder.Instance.UnloadContent();
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            _tankView.Update(gameTime, currentMouseState);
            _topBarView.Update(gameTime, currentMouseState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix)
        {
            // Draw the fish tank
            spriteBatch.Begin(
                    samplerState: SamplerState.LinearClamp,
                    blendState: BlendState.AlphaBlend,
                    transformMatrix: _tankView.PreTransformMatrix * transformMatrix);

            _tankView.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            // Draw the top item bar
           spriteBatch.Begin(
                    samplerState: SamplerState.LinearClamp,
                    blendState: BlendState.AlphaBlend,
                    transformMatrix: transformMatrix);

            _topBarView.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            return GetViewContainingPoint(mouseEvent.Position)?.MouseEvent(mouseEvent) ?? false;
        }

        private Component GetViewContainingPoint(Point point)
        {
            if (_tankView.Area.Contains(point))
            {
                return _tankView;
            }
            if (_topBarView.Area.Contains(point))
            {
                return _topBarView;
            }
            return null;
        }

        private void PurchaseGoldFish(object sender, EventArgs e)
        {
            _tankView.AddGoldFish();
        }

        private TankComponent _tankView;

        private ItemBarComponent _topBarView;
    }
}
