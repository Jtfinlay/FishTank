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
using FishTank.Models;
using FishTank.Models.Levels;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Components
{
    public class ItemBarComponent : Component
    {
        public event EventHandler<PurchaseEventArgs> OnPurchaseFish;

        public int GoldAmount
        {
            get
            {
                return _coinBank.GoldAmount;
            }
            set
            {
                _coinBank.GoldAmount = value;
            }
        }

        public ItemBarComponent(Level level)
        {
            _level = level;

            Area = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualBarHeight);

            Rectangle paddedArea = Area.ApplyPadding(Constants.Padding, Alignment.Right | Alignment.Bottom);
            _coinBank = new CoinBankBar(new Rectangle(paddedArea.Right - (225 + Constants.Padding), Constants.Padding * 3 + 50, 225, 50));

            GoldAmount = level.InitialGold;
        }

        public override void LoadContent()
        {
            ContentBuilder.Instance.LoadFontByName(_fontName);

            Rectangle paddedArea = Area.ApplyPadding(Constants.Padding, Alignment.Right | Alignment.Bottom);
            _menuButton = new ButtonComponent(new Rectangle(paddedArea.Right - (225 + Constants.Padding), Constants.Padding, 225, 50), ContentBuilder.Instance.GetString("Menu"), _fontName);
            _menuButton.OnClick += OnMenuButtonClicked;

            _menuButton.LoadContent();
            _coinBank.LoadContent();

            _buttons = new List<TopbarItem>();
            for (int i = 0; i < Constants.TopBarItems; i++)
            {
                var topBarItem = new TopbarItem(_level.Items[i], new Rectangle(i * Area.Height, 0, Area.Height, Area.Height));
                topBarItem.LoadContent();
                topBarItem.OnPurchased += OnItemPurchase;
                _buttons.Add(topBarItem);
            }

            ContentBuilder.Instance.CreateRectangleTexture(_backgroundAssetName, Area.Width, Area.Height);
        }

        public override void UnloadContent()
        {
            _menuButton.OnClick -= OnMenuButtonClicked;
            _menuButton.UnloadContent();
            _coinBank.UnloadContent();
            _buttons.ForEach((item) =>
            {
                item.OnPurchased -= OnItemPurchase;
                item.UnloadContent();
            });
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_backgroundAssetName), new Vector2(0, 0), Color.Coral);
            _buttons.ForEach((button) => button.Draw(gameTime, spriteBatch));

            spriteBatch.DrawString(ContentBuilder.Instance.LoadFontByName(_fontName), _level.LevelName, Area, Alignment.Right | Alignment.Bottom, Color.Black);
            _menuButton.Draw(gameTime, spriteBatch);
            _coinBank.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            _menuButton.Update(gameTime, currentMouseState);
            _buttons.ForEach((button) => button.Update(gameTime, currentMouseState));
        }

        public override bool MouseEvent(InputEvent mouseEvent)
        {
            if (_menuButton.Area.Contains(mouseEvent.Position))
            {
                return _menuButton.MouseEvent(mouseEvent);
            }
            return _buttons.Where((button) => button.Area.Contains(mouseEvent.Position)).FirstOrDefault()?.MouseEvent(mouseEvent) ?? false;
        }

        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
        }

        private void OnItemPurchase(object sender, PurchaseEventArgs e)
        {
            TopbarItem item = sender as TopbarItem;
            if (item.GoldValue > GoldAmount)
            {
                // not enough gold
                return;
            }
            switch (item.ItemType)
            {
                case LevelItemType.ClownFish:
                case LevelItemType.PiranhaFish:
                    GoldAmount -= item.GoldValue;
                    OnPurchaseFish?.Invoke(this, e);
                    break;
            }
        }

        /// <summary>
        /// Object containing level-specific data for the game
        /// </summary>
        private Level _level;

        private readonly string _backgroundAssetName = TextureNames.ItemBarComponentAsset;

        private readonly string _fontName = FontNames.Arial_20;

        private List<TopbarItem> _buttons;

        private ButtonComponent _menuButton;

        private CoinBankBar _coinBank;
    }
}
