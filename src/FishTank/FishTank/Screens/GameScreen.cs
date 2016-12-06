//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishTank.Screens
{
    public class GameScreen : IScreen
    {
        public Rectangle Area => Constants.VirtualArea;

        public GameScreen()
        {
            _topBarView = new TopBarView();
            _tankView = new TankView(0, _topBarView.Area.Height);

            _topBarView.OnPurchaseFish += PurchaseGoldFish;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _tankView.LoadContent(graphicsDevice, content);
            _topBarView.LoadContent(graphicsDevice, content);

            _tankView.AddGoldFish();
        }

        public void UnloadContent()
        {
            _topBarView.OnPurchaseFish -= PurchaseGoldFish;

            _tankView.UnloadContent();
            _topBarView.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            _tankView.Update(gameTime);
            _topBarView.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transformMatrix)
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

        public void MouseHover(MouseState mouseState)
        {
            GetViewContainingPoint(mouseState.Position)?.MouseHover(mouseState);
        }

        public void MouseClick(MouseState mouseState)
        {
            GetViewContainingPoint(mouseState.Position)?.MouseClick(mouseState);
        }

        public void MouseRelease(MouseState mouseState)
        {
            GetViewContainingPoint(mouseState.Position)?.MouseRelease(mouseState);
        }

        private IComponent GetViewContainingPoint(Point point)
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

        private TankView _tankView;

        private TopBarView _topBarView;
    }
}
