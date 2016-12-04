//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.ViewAdapters;
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
            _models = new List<IInteractable>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
            _backgroundTexture = Content.Load<Texture2D>("RollingHills.png");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _models.Add(new GoldFish(GraphicsDevice));

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
            float frameRateChange = gameTime.ElapsedGameTime.Milliseconds / ExpectedMillisecondsPerFrame;

            HandleInputs();

            // Clear out stale interactables.
            _models.RemoveAll((model) => model.State == InteractableState.Dead);

            // Update interactables
            foreach (IInteractable model in _models)
            {
                model.Update(_models);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(
                samplerState: SamplerState.LinearClamp,
                blendState: BlendState.AlphaBlend,
                transformMatrix: _viewportAdapter.GetScaleMatrix());

            _spriteBatch.Draw(_backgroundTexture, new Vector2(-200, -200));

            foreach (IInteractable model in _models)
            {
                model.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInputs()
        {
            // Handle mouse input events
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var mousePosition = _viewportAdapter.PointToScreen(_currentMouseState.Position);
            var virtualMousePosition = new Vector2(mousePosition.X, mousePosition.Y);
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                _models.Add(new Pellet(GraphicsDevice, virtualMousePosition));
            }
        }

        private List<IInteractable> _models;

        private Texture2D _backgroundTexture;

        private BoxingViewportAdapter _viewportAdapter;

        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// Spritebatch is used to draw textures on the canvas
        /// </summary>
        private SpriteBatch _spriteBatch;

        // Mouse states used to track Mouse button press
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

    }
}
