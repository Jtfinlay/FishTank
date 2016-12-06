//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FishTank.Views
{
    public class TankView : IView
    {
        public Matrix PostScaleTransform { get; private set; }

        public Rectangle Area { get; private set; }

        public TankView(int offsetX, int offsetY)
        {
            PostScaleTransform = Matrix.CreateTranslation(new Vector3(offsetX, offsetY, 0));
            _models = new List<IInteractable>();

            Area = new Rectangle(offsetX, offsetY, Constants.VirtualWidth, Constants.VirtualHeight);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;

            _backgroundTexture = content.Load<Texture2D>("RollingHills.png");
        }

        public void AddGoldFish()
        {
            _models.Add(new GoldFish(_graphicsDevice));
        }

        public void UnloadContent() { }

        public void Update(GameTime gameTime)
        {
            // Clear out stale interactables.
            _models.RemoveAll((model) => model.State == InteractableState.Discard);

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

        public void MouseHover(MouseState mouseState) { }

        public void MouseClick(MouseState mouseState)
        {
            // The tank is offset from the top bar. Apply this transformation to the mouse position
            var virtualPosition = mouseState.Position.ToVector2();
            var translatedPosition = Vector2.Transform(virtualPosition, Matrix.Invert(PostScaleTransform));

            _models.Add(new Pellet(_graphicsDevice, translatedPosition));
        }

        public void MouseRelease(MouseState mouseState) { }

        private Texture2D _backgroundTexture;

        private List<IInteractable> _models;

        private GraphicsDevice _graphicsDevice;
    }
}
