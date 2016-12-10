//
// Copyright - James Finlay
// 

using FishTank.Models;
using FishTank.Models.Levels;
using FishTank.Utilities;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Components
{
    public class ItemBarComponent : Component
    {
        public event EventHandler OnPurchaseFish;

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
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _gameStatusBar.LoadContent(graphicsDevice, content);
            _buttons = new List<TopbarItem>();
            for (int i = 0; i < Constants.TopBarItems; i++)
            {
                var topBarItem = new TopbarItem(_level.Items[i], new Rectangle(i * Area.Height, 0, Area.Height, Area.Height));
                topBarItem.LoadContent(graphicsDevice, content);
                topBarItem.OnPurchased += OnItemPurchase;
                _buttons.Add(topBarItem);
            }

            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);
            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Coral;
            }
            rect.SetData(data);
            _texture = rect;
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
            spriteBatch.Draw(_texture, new Vector2(0, 0), null);
            _buttons.ForEach((button) => button.Draw(gameTime, spriteBatch));
            _gameStatusBar.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            return _buttons.Where((button) => button.Area.Contains(mouseEvent.Position)).FirstOrDefault()?.MouseEvent(mouseEvent) ?? false;
        }

        private void OnItemPurchase(object sender, EventArgs e)
        {
            TopbarItem item = sender as TopbarItem;
            if (item.GoldValue > GoldAmount)
            {
                // not enough gold
                return;
            }
            if (item.ItemType == LevelItemTypes.GuppyFish)
            {
                GoldAmount -= item.GoldValue;
                OnPurchaseFish?.Invoke(this, null);
            }
        }

        private Level _level;

        private Texture2D _texture;

        private List<TopbarItem> _buttons;

        private GameStatusBar _gameStatusBar;

        private Matrix _postScaleTransform = Matrix.CreateTranslation(0,0,0);
    }
}
