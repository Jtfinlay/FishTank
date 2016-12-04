//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Interfaces;
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
    public class Game1 : Game
    {
        public static readonly float ExpectedFramesPerSecond = 60f;

        public static readonly float ExpectedMillisecondsPerFrame = 1000f / ExpectedFramesPerSecond;

        public Game1()
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

            _gameView = new GameView();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameView.LoadContent(_graphics.GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //float frameRateChange = gameTime.ElapsedGameTime.Milliseconds / ExpectedMillisecondsPerFrame;
            MouseState mouseState = Mouse.GetState();

            Point virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.Position);
            MouseState virtualMouseState = mouseState.SetPosition(virtualMousePosition.ToVector2());

            _gameView.Update(gameTime, virtualMouseState);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw top bar
            //_spriteBatch.Begin(
            //    samplerState: SamplerState.LinearClamp,
            //    blendState: BlendState.AlphaBlend,
            //    transformMatrix: _viewportAdapter.GetScaleMatrix());



            //_spriteBatch.End();

            // Draw Gameview
            _spriteBatch.Begin(
                samplerState: SamplerState.LinearClamp,
                blendState: BlendState.AlphaBlend,
                transformMatrix: _viewportAdapter.GetScaleMatrix());

            _gameView.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }



        /// <summary>
        /// Spritebatch is used to draw textures on the canvas
        /// </summary>
        private SpriteBatch _spriteBatch;

        private BoxingViewportAdapter _viewportAdapter;

        private GraphicsDeviceManager _graphics;

        private GameView _gameView;
    }
}
