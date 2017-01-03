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

using FishTank.Drawing;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

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
            BoundaryBox = new Rectangle2(position, new Vector2(_width, _height));

            LoadAssets();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (State == InteractableState.Discard)
            {
                // coin is destroyed but awaiting cleanup. Don't draw
                return;
            }

            Animation animation = null;
            if (CoinValue >= DiamondCoinValue) animation = _diamondAnimation;
            else if (CoinValue >= GoldCoinValue) animation = _goldAnimation;
            else animation = _silverAnimation;

            animation.Draw(spriteBatch, gameTime, BoundaryBox.Location, SpriteEffects.None);
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

        private void LoadAssets()
        {
            _spriteSheet = new SpriteSheet(_spriteSheetAssetName, BoundaryBox.Size.ToPoint());

            // Silver
            var frames = new List<Point>()
            {
                new Point(0,0),
                new Point(0, _height),
                new Point(0, _height * 2),
                new Point(0, _height * 3),
                new Point(0, _height * 4),
            };
            _silverAnimation = new Animation(_spriteSheet, frames);

            // Gold
            frames = frames.Select((point) => { point += new Point(_width, 0); return point; }).ToList();
            _goldAnimation = new Animation(_spriteSheet, frames);

            // Diamond
            frames = new List<Point>()
            {
                new Point(_width*2,0),
                new Point(_width*2, _height),
                new Point(_width*2, _height * 2),
                new Point(_width*2,0),
                new Point(_width*2, _height * 3),
            };
            _diamondAnimation = new Animation(_spriteSheet, frames);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_spriteSheetAssetName);
        }

        private const int _width = 30;

        private const int _height = 30;

        private Animation _silverAnimation;

        private Animation _goldAnimation;

        private Animation _diamondAnimation;

        private SpriteSheet _spriteSheet;

        private readonly string _spriteSheetAssetName = "sheets\\coin_sheet.png";
    }
}
