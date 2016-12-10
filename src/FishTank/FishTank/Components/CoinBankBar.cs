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

using FishTank.Utilities;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Components
{
    public class CoinBankBar : Component
    {
        public int GoldAmount { get; set; } = 0;
        
        public CoinBankBar(Rectangle area)
        {
            Area = area;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            var coinRect = new Texture2D(graphicsDevice, 20, 20);
            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }
            coinRect.SetData(data);
            _coinIconTexture = coinRect;

            var bgRect = new Texture2D(graphicsDevice, Area.Width, Area.Height);
            data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }
            bgRect.SetData(data);
            _backgroundTexture = bgRect;
            _fishFont = content.Load<SpriteFont>("Arial_20");
        }

        public override void UnloadContent()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, Area.Location.ToVector2(), Color.Black);
            spriteBatch.DrawString(_fishFont, _goldAmountString, Area.ApplyPadding(Constants.Padding, Alignment.Right), Alignment.Right, Color.White);

            var coinIconPosition = Area.Location.ToVector2() + new Vector2(Constants.Padding, (Area.Height / 2) - (_coinIconTexture.Height / 2));
            spriteBatch.Draw(_coinIconTexture, coinIconPosition, Color.Gold);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            return false;
        }

        private string _goldAmountString => $"{GoldAmount}g";

        private SpriteFont _fishFont;

        private Texture2D _backgroundTexture;

        private Texture2D _coinIconTexture;
    }
}
