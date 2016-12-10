//
// Copyright - James Finlay
// 

using FishTank.Screens;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using FishTank.ViewAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishTank
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AppController : Game
    {
        public AppController()
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
            _screen = new MainMenuScreen();
            _screen.OnNavigate += NavigateToScreen;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Constants.VirtualTotalWidth, Constants.VirtualTotalHeight);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _screen.LoadContent(GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _screen.OnNavigate -= NavigateToScreen;
            _screen.UnloadContent();
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

            // Perform mouse click events on the display
            if (_screen.Area.Contains(_currentMouseState.Position) || _screen.Area.Contains(_previousMouseState.Position))
            {
                MouseAction action = MouseAction.Hover;
                if (_currentMouseState.LeftButton == _previousMouseState.LeftButton)
                {
                    action = MouseAction.Hover;
                }
                else if (_currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    action = MouseAction.Click;
                }
                else if (_currentMouseState.LeftButton == ButtonState.Released)
                {
                    action = MouseAction.Release;
                }
                _screen.MouseEvent(new MouseEvent(_currentMouseState, action));
            }

            _screen.Update(gameTime, _currentMouseState);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _screen.Draw(gameTime, _spriteBatch, _viewportAdapter.GetScaleMatrix());

            base.Draw(gameTime);
        }

        /// <summary>
        /// Invoked to switch to a new menu or game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToScreen(object sender, NavigationEventArgs e)
        {
            _screen.OnNavigate -= NavigateToScreen;
            _screen.UnloadContent();

            _screen = Activator.CreateInstance(e.Target, e) as Screen;
            _screen.LoadContent(GraphicsDevice, Content);
            _screen.OnNavigate += NavigateToScreen;
        }


        private Screen _screen;

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
