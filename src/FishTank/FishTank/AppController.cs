//
//  Copyright 2017 James Finlay
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

using FishTank.Instrumentation;
using FishTank.Screens;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using FishTank.ViewAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
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
            Log.LogVerbose("Initializing");

            IsMouseVisible = true;
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap;

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
            Log.LogVerbose("Loading content");

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
            Log.LogVerbose("Unloading content");

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
            if (gameTime.IsRunningSlowly)
            {
                Log.LogVerbose($"Running slow: {gameTime.ElapsedGameTime}");
            }

            // Touch screen logic
            GestureSample gesture = default(GestureSample);
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap || gesture.GestureType == GestureType.DoubleTap)
                {
                    Point virtualTouchPosition = _viewportAdapter.PointToScreen(gesture.Position.ToPoint());
                    _screen.MouseEvent(new InputEvent(InputAction.TouchTap, virtualTouchPosition));
                }
            }

            // Mouse logic
            MouseState mouseState = Mouse.GetState();

            Point virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.Position);
            MouseState virtualMouseState = mouseState.SetPosition(virtualMousePosition.ToVector2());

            _previousMouseState = _currentMouseState;
            _currentMouseState = virtualMouseState;

            // Perform mouse click events on the display
            if (_screen.Area.Contains(_currentMouseState.Position) || _screen.Area.Contains(_previousMouseState.Position))
            {
                InputAction action = InputAction.Hover;
                if (_currentMouseState.LeftButton == _previousMouseState.LeftButton)
                {
                    action = InputAction.Hover;
                }
                else if (_currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    action = InputAction.Click;
                }
                else if (_currentMouseState.LeftButton == ButtonState.Released)
                {
                    action = InputAction.Release;
                }
                _screen.MouseEvent(new InputEvent(_currentMouseState, action));
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
            if (gameTime.IsRunningSlowly)
            {
                Log.LogVerbose($"Running slow: {gameTime.ElapsedGameTime}");
            }

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
            Log.LogVerbose($"Navigating to screen: {e.Target.ToString()}");
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
