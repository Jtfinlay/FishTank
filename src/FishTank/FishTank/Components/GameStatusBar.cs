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

        public override void LoadContent()
        {
            ContentBuilder.Instance.LoadFontByName(_fontName);
            _menuButton.LoadContent();
            _coinBank.LoadContent();
        }

        public override bool MouseEvent(InputEvent mouseEvent)
        {
            return false;
        }

        public override void UnloadContent() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentBuilder.Instance.LoadFontByName(_fontName), _levelName, Area, Alignment.Right | Alignment.Bottom, Color.Black);
            _menuButton.Draw(gameTime, spriteBatch);
            _coinBank.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState) { }

        private readonly string _fontName = FontNames.Arial_20;

        private ButtonComponent _menuButton;

        private CoinBankBar _coinBank;

        private string _levelName;
    }
}
