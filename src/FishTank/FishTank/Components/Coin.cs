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

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using FishTank.Utilities.Inputs;
using System;
using FishTank.Content;

namespace FishTank.Components
{
    /// <summary>
    /// Describes a collectable <see cref="Coin"/> object dropped by fish.
    /// </summary>
    public class Coin : IInteractable, IClickable
    {
        public event EventHandler OnClick;

        public Rectangle2 BoundaryBox { get; private set; }

        public InteractableState State { get; private set; }

        public Rectangle Area => BoundaryBox.ToRectangle();

        public int CoinValue { get; private set; }

        public const int SilverCoinValue = 50;

        public const int GoldCoinValue = 100;

        public const int DiamondCoinValue = 150;

        public Coin(Vector2 position, int coinValue)
        {
            CoinValue = coinValue;
            BoundaryBox = new Rectangle2(position, new Vector2(20, 20));

            ContentBuilder.Instance.CreateRectangleTexture(_assetName, BoundaryBox.ToRectangle().Width, BoundaryBox.ToRectangle().Height);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (State == InteractableState.Discard)
            {
                // coin is destroyed but awaiting cleanup. Don't draw
                return;
            }

            Color color = Color.White;
            if (CoinValue >= DiamondCoinValue) color = Color.LightBlue;
            else if (CoinValue >= GoldCoinValue) color = Color.Gold;
            else color = Color.Silver;

            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), BoundaryBox.Location, color);
        }

        public void Update(List<IInteractable> models, GameTime gameTime)
        {
            var position = Vector2.Add(BoundaryBox.Location, new Vector2(0, Constants.FallSpeed));
            BoundaryBox = new Rectangle2(position, BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                State = InteractableState.Discard;
            }
        }

        public bool MouseEvent(InputEvent mouseEvent)
        {
            if (mouseEvent.Action == InputAction.Click ||
                mouseEvent.Action == InputAction.TouchTap)
            {
                return Gather();
            }
            return false;
        }

        /// <summary>
        /// Consume the coin and invoke clicked event.
        /// </summary>
        /// <returns>Boolean indicating whether coin was consumed.</returns>
        public bool Gather()
        {
            if (State == InteractableState.Discard ||
                State == InteractableState.Dead)
            {
                return false;
            }

            OnClick?.Invoke(this, null);
            State = InteractableState.Discard;
            return true;
        }

        private readonly string _assetName = TextureNames.CoinAsset;
    }
}
