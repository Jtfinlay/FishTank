//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Components
{
    public class TankComponent : IComponent
    {
        public Matrix PreTransformMatrix { get; private set; }

        public Rectangle Area { get; private set; }

        public TankComponent(int offsetX, int offsetY)
        {
            PreTransformMatrix = Matrix.CreateTranslation(new Vector3(offsetX, offsetY, 0));
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

        public void MouseEvent(MouseEvent mouseEvent)
        {
            switch (mouseEvent.Action)
            {
                case MouseAction.Click:
                    // The tank is offset from the top bar. Apply this transformation to the mouse position
                    var translatedPosition = Vector2.Transform(mouseEvent.Location, Matrix.Invert(PreTransformMatrix));
                    _models.Add(new Pellet(_graphicsDevice, translatedPosition));
                    break;
                case MouseAction.Hover:
                case MouseAction.HoverExit:
                case MouseAction.Release:
                default:
                    break;
            }
        }

        private Texture2D _backgroundTexture;

        private List<IInteractable> _models;

        private GraphicsDevice _graphicsDevice;
    }
}
