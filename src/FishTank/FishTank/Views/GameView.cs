//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FishTank.Views
{
    public class GameView : IView
    {
        public GameView()
        {
            _models = new List<IInteractable>();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;

            _backgroundTexture = content.Load<Texture2D>("RollingHills.png");
            _models.Add(new GoldFish(_graphicsDevice));
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, MouseState virtualMouseState)
        {
            HandleInputs(virtualMouseState);

            // Clear out stale interactables.
            _models.RemoveAll((model) => model.State == InteractableState.Dead);

            // Update interactables
            foreach (IInteractable model in _models)
            {
                model.Update(_models);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0));

            foreach (IInteractable model in _models)
            {
                model.Draw(spriteBatch);
            }
        }

        private void HandleInputs(MouseState virtualMouseState)
        {
            // Handle mouse input events
            _previousMouseState = _currentMouseState;
            _currentMouseState = virtualMouseState;

            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                _models.Add(new Pellet(_graphicsDevice, _currentMouseState.Position.ToVector2()));
            }
        }

        private Texture2D _backgroundTexture;

        private List<IInteractable> _models;

        private GraphicsDevice _graphicsDevice;
        
        // Mouse states used to track Mouse button press
        private MouseState _currentMouseState;

        private MouseState _previousMouseState;

    }
}
