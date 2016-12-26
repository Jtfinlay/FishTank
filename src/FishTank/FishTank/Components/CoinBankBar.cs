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
using FishTank.Utilities;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
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

        public override void LoadContent()
        {
            // Preload assets
            ContentBuilder.Instance.CreateRectangleTexture(_assetName, Area.Width, Area.Height);
            ContentBuilder.Instance.LoadTextureByName(_coinAssetName);
            ContentBuilder.Instance.LoadFontByName(_fontAssetName);
        }

        public override void UnloadContent() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), Area.Location.ToVector2(), Color.Black);
            spriteBatch.DrawString(ContentBuilder.Instance.LoadFontByName(_fontAssetName), _goldAmountString, Area.ApplyPadding(Constants.Padding, Alignment.Right), Alignment.Right, Color.White);


            Texture2D coinAsset = ContentBuilder.Instance.LoadTextureByName(_coinAssetName);
            var coinIconPosition = Area.Location.ToVector2() + new Vector2(Constants.Padding, (Area.Height / 2) - (coinAsset.Height / 2));
            spriteBatch.Draw(coinAsset, coinIconPosition, Color.Gold);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        public override bool MouseEvent(InputEvent mouseEvent)
        {
            return false;
        }

        private string _goldAmountString => $"{GoldAmount}g";

        private readonly string _assetName = TextureNames.CoinBankBarAsset;

        private readonly string _coinAssetName = TextureNames.CoinAsset;

        private readonly string _fontAssetName = FontNames.Arial_20;
    }
}
