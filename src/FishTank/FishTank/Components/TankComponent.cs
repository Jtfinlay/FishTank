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
            GuppyFish fish = new GuppyFish(_graphicsDevice, _content);
            fish.OnCoinDrop += Fish_OnCoinDrop;
            _models.Add(fish);
        }

        public override void UnloadContent()
        {
            _models.ForEach((model) =>
            {
                if (model is GuppyFish)
                {
                    (model as GuppyFish).OnCoinDrop -= Fish_OnCoinDrop;
                }
                else if (model is Coin)
                {
                    (model as Coin).OnClick -= Coin_OnClick;
                }
            });
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            // Clear out stale interactables.
            _models.RemoveAll((model) => model.State == InteractableState.Discard);

            // Update interactables
            // Create new list since our updates may trigger new items
            foreach (IInteractable model in new List<IInteractable>(_models))
            {
                model.Update(_models, gameTime);
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

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            switch (mouseEvent.Action)
            {
                case MouseAction.Click:
                    // The tank is offset from the top bar. Apply this transformation to the mouse position
                    var translatedPosition = Vector2.Transform(mouseEvent.Location, Matrix.Invert(PreTransformMatrix));
                    mouseEvent.Position = translatedPosition.ToPoint();

                    foreach (IInteractable model in _models)
                    {
                        // First try to pass mouse click on to clickable models. Complete on first successful item
                        IClickable clickable = model as IClickable;
                        if (clickable?.Area.Contains(mouseEvent.Location) ?? false)
                        {
                            if (clickable.MouseEvent(mouseEvent))
                            {
                                return true;
                            }
                        }
                    }
                    _models.Add(new Pellet(_graphicsDevice, translatedPosition));
                    return true;
                case MouseAction.Hover:
                case MouseAction.HoverExit:
                case MouseAction.Release:
                default:
                    break;
            }
            return false;
        }

        private void Fish_OnCoinDrop(object sender, System.EventArgs e)
        {
            Coin coin = new Coin(_graphicsDevice, (sender as GuppyFish).BoundaryBox.Center.ToVector2());
            coin.OnClick += Coin_OnClick;
            _models.Add(coin);
        }

        private void Coin_OnClick(object sender, System.EventArgs e)
        {
            // TODO
        }

        private Rectangle _drawArea;

        private Texture2D _backgroundTexture;

        private List<IInteractable> _models;

        private GraphicsDevice _graphicsDevice;

        private ContentManager _content;
    }
}
