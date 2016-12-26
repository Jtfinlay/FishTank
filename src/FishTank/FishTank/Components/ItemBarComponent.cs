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
                return _gameStatusBar.GoldAmount;
            }
            set
            {
                _gameStatusBar.GoldAmount = value;
            }
        }

        public ItemBarComponent(Level level)
        {
            _level = level;

            Area = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualBarHeight);
            _gameStatusBar = new GameStatusBar(_level, Area);

            GoldAmount = level.InitialGold;
        }

        public override void LoadContent()
        {
            _gameStatusBar.LoadContent();
            _buttons = new List<TopbarItem>();
            for (int i = 0; i < Constants.TopBarItems; i++)
            {
                var topBarItem = new TopbarItem(_level.Items[i], new Rectangle(i * Area.Height, 0, Area.Height, Area.Height));
                topBarItem.LoadContent();
                topBarItem.OnPurchased += OnItemPurchase;
                _buttons.Add(topBarItem);
            }

            ContentBuilder.Instance.CreateRectangleTexture(_assetName, Area.Width, Area.Height);
        }

        public override void UnloadContent()
        {
            _gameStatusBar.UnloadContent();
            _buttons.ForEach((item) =>
            {
                item.OnPurchased -= OnItemPurchase;
                item.UnloadContent();
            });
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), new Vector2(0, 0), Color.Coral);
            _buttons.ForEach((button) => button.Draw(gameTime, spriteBatch));
            _gameStatusBar.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        public override bool MouseEvent(InputEvent mouseEvent)
        {
            return _buttons.Where((button) => button.Area.Contains(mouseEvent.Position)).FirstOrDefault()?.MouseEvent(mouseEvent) ?? false;
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
                case LevelItemType.GuppyFish:
                case LevelItemType.PiranhaFish:
                    GoldAmount -= item.GoldValue;
                    OnPurchaseFish?.Invoke(this, e);
                    break;
            }
        }

        private Level _level;

        private readonly string _assetName = TextureNames.ItemBarComponentAsset;

        private List<TopbarItem> _buttons;

        private GameStatusBar _gameStatusBar;
    }
}
