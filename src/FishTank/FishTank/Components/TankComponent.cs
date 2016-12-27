//
//  Copyright 2016 James Finlay
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

using FishTank.Content;
using FishTank.Models;
using FishTank.Models.Interfaces;
using FishTank.Models.Levels;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FishTank.Components
{
    public class TankComponent : Component
    {
        public event EventHandler OnCoinClick;

        public Matrix PreTransformMatrix { get; private set; }

        public TankComponent(int offsetX, int offsetY)
        {
            PreTransformMatrix = Matrix.CreateTranslation(new Vector3(offsetX, offsetY, 0));
            _models = new List<IInteractable>();

            Area = new Rectangle(offsetX, offsetY, Constants.VirtualWidth, Constants.VirtualHeight);
            _drawArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
        }

        public override void LoadContent()
        {
            // Preload content
            ContentBuilder.Instance.LoadTextureByName(_backgroundAssetName);
        }

        public void CreateFish(LevelItemType type)
        {
            Fish fish = null;
            switch (type)
            {
                case LevelItemType.GuppyFish:
                    fish = new GuppyFish();
                    break;
                case LevelItemType.PiranhaFish:
                    fish = new Piranha();
                    break;
                case LevelItemType.FeederFish:
                    fish = new FeederFish();
                    break;
                case LevelItemType.CoinCrab:
                    fish = new CoinCrab();
                    break;
                default:
                    throw new ArgumentException($"Unexpected item type: {type}");
            }
            fish.OnItemDrop += Fish_OnItemDrop;
            _models.Add(fish);
        }

        public override void UnloadContent()
        {
            _models.ForEach((model) =>
            {
                if (model is Fish)
                {
                    (model as Fish).OnItemDrop -= Fish_OnItemDrop;
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
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_backgroundAssetName), destinationRectangle: _drawArea);

            foreach (IInteractable model in _models)
            {
                model.Draw(spriteBatch);
            }
        }

        public override bool MouseEvent(InputEvent mouseEvent)
        {
            switch (mouseEvent.Action)
            {
                case InputAction.Click:
                case InputAction.TouchTap:
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
                    _models.Add(new Pellet(translatedPosition, new Vector2(0,0)));
                    return true;
                case InputAction.Hover:
                case InputAction.HoverExit:
                case InputAction.Release:
                default:
                    break;
            }
            return false;
        }

        private void Fish_OnItemDrop(object sender, ItemDropEventArgs e)
        {
            if (e.ItemType == typeof(Coin))
            {
                Coin coin = new Coin(e.Position);
                coin.OnClick += Coin_OnClick;
                _models.Add(coin);
            }
            else if (e.ItemType == typeof(Pellet))
            {
                _models.Add(new Pellet(e.Position, e.Velocity));
            }
        }

        private void Coin_OnClick(object sender, System.EventArgs e)
        {
            OnCoinClick?.Invoke(sender, e);
        }

        private Rectangle _drawArea;

        private List<IInteractable> _models;

        private readonly string _backgroundAssetName = "backgrounds\\background2.png";
    }
}
