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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishTank.Components
{
    public class ButtonComponent : Component
    {
        public event EventHandler OnClick;

        public string FontName { get; private set; }

        public string ButtonText { get; set; }

        public ButtonComponent(Rectangle area, string buttonText, string fontName = null)
        {
            fontName = string.IsNullOrWhiteSpace(fontName) ? FontNames.FishFingers_70 : fontName;

            Area = area;
            ButtonText = buttonText;
            FontName = fontName;
        }

        public override void LoadContent()
        {
            // Preload assets
            ContentBuilder.Instance.CreateRectangleTexture(_backgroundAssetName, Area.Width, Area.Height);
            ContentBuilder.Instance.LoadFontByName(FontName);
        }

        public override void UnloadContent() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color backgroundColor = _isMouseOver ? Color.Blue : Color.LightBlue;
            Color fontColor = _isMouseOver ? Color.White : Color.Black;

            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_backgroundAssetName), Area.Location.ToVector2(), backgroundColor);
            spriteBatch.DrawString(ContentBuilder.Instance.LoadFontByName(FontName), ButtonText, Area, Alignment.Center, fontColor);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            _isMouseOver = Area.Contains(currentMouseState.Position);
        }

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            if (mouseEvent.Action == MouseAction.Click)
            {
                OnClick?.Invoke(this, null);
                return true;
            }
            return false;
        }

        private bool _isMouseOver;

        private string _backgroundAssetName => $"ButtonComponentAsset{Area.Width}x{Area.Height}";
    }
}
