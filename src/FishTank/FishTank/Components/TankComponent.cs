//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FishTank.Components
{
    public class TankComponent : Component
    {
        public Matrix PreTransformMatrix { get; private set; }

        public TankComponent(int offsetX, int offsetY)
        {
            PreTransformMatrix = Matrix.CreateTranslation(new Vector3(offsetX, offsetY, 0));
            _models = new List<IInteractable>();

            Area = new Rectangle(offsetX, offsetY, Constants.VirtualWidth, Constants.VirtualHeight);
            _drawArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _content = content;
            _backgroundTexture = content.Load<Texture2D>("backgrounds\\background2.png");
        }

        public void AddGoldFish()
        {
            _models.Add(new GuppyFish(_graphicsDevice, _content));
        }

        public override void UnloadContent() { }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            // Clear out stale interactables.
            _models.RemoveAll((model) => model.State == InteractableState.Discard);

            // Update interactables
            foreach (IInteractable model in _models)
            {
                model.Update(_models);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, destinationRectangle: _drawArea);

            foreach (IInteractable model in _models)
            {
                model.Draw(spriteBatch);
            }
        }

        public override void MouseEvent(MouseEvent mouseEvent)
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

        private Rectangle _drawArea;

        private Texture2D _backgroundTexture;

        private List<IInteractable> _models;

        private GraphicsDevice _graphicsDevice;

        private ContentManager _content;
    }
}
