//
// Copyright - James Finlay
// 

using FishTank.Models.Levels;
using FishTank.Utilities;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishTank.Components
{
    public class GameStatusBar : Component
    {
        public event EventHandler OnMenuClicked;

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

        public GameStatusBar(Level level, Rectangle parentArea)
        {
            _levelName = level.LevelName;
            Area = parentArea.ApplyPadding(Constants.Padding, Alignment.Right | Alignment.Bottom);
            _menuButton = new ButtonComponent(new Rectangle(Area.Right - (225 + Constants.Padding), Constants.Padding, 225, 50), "Menu", "Arial_20");
            _coinBank = new CoinBankBar(new Rectangle(Area.Right - (225 + Constants.Padding), Constants.Padding * 3 + 50, 225, 50));
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _fishFont = content.Load<SpriteFont>("Arial_20");
            _menuButton.LoadContent(graphicsDevice, content);
            _coinBank.LoadContent(graphicsDevice, content);
        }

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            return false;
        }

        public override void UnloadContent()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_fishFont, _levelName, Area, Alignment.Right | Alignment.Bottom, Color.Black);
            _menuButton.Draw(gameTime, spriteBatch);
            _coinBank.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        private SpriteFont _fishFont;

        private ButtonComponent _menuButton;

        private CoinBankBar _coinBank;

        private string _levelName;
    }
}
