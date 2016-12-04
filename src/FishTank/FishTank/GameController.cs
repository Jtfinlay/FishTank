//
// Copyright - James Finlay
// 

using FishTank.Utilities;
using FishTank.ViewAdapters;
using FishTank.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FishTank
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {

        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            _topBarView = new TopBarView();
            _tankView = new TankView(0, _topBarView.Area.Height);

            _topBarView.OnPurchaseFish += ItemBarView_OnPurchaseFish;

            base.Initialize();
        }

        private void ItemBarView_OnPurchaseFish(object sender, System.EventArgs e)
        {
            _tankView.AddGoldFish();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _tankView.LoadContent(_graphics.GraphicsDevice, Content);
            _topBarView.LoadContent(_graphics.GraphicsDevice, Content);

            _tankView.AddGoldFish();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _topBarView.OnPurchaseFish -= ItemBarView_OnPurchaseFish;

            _tankView.UnloadContent();
            _topBarView.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            Point virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.Position);
            MouseState virtualMouseState = mouseState.SetPosition(virtualMousePosition.ToVector2());

            _previousMouseState = _currentMouseState;
            _currentMouseState = virtualMouseState;

            foreach (IView view in new List<IView>() { _tankView, _topBarView })
            {
                if (_currentMouseState.Position.Within(view.Area))
                {
                    // Mouse is in the view's area. Notify whether it is a hover, click, or release.
                    if (_currentMouseState.LeftButton == _previousMouseState.LeftButton)
                    {
                        view.MouseHover(_currentMouseState);
                    }
                    else if (_currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        view.MouseClick(_currentMouseState);
                    }
                    else
                    {
                        view.MouseRelease(_currentMouseState);
                    }
                }

                view.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(
                    samplerState: SamplerState.LinearClamp,
                    blendState: BlendState.AlphaBlend,
                    transformMatrix: _tankView.PostScaleTransform * _viewportAdapter.GetScaleMatrix());

            _tankView.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin(
                    samplerState: SamplerState.LinearClamp,
                    blendState: BlendState.AlphaBlend,
                    transformMatrix: _viewportAdapter.GetScaleMatrix());

            _topBarView.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private TankView _tankView;

        private TopBarView _topBarView;

        /// <summary>
        /// Spritebatch is used to draw textures on the canvas
        /// </summary>
        private SpriteBatch _spriteBatch;

        private BoxingViewportAdapter _viewportAdapter;

        private GraphicsDeviceManager _graphics;

        private MouseState _currentMouseState;

        private MouseState _previousMouseState;
    }
}
