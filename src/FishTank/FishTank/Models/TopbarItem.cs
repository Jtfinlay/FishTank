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

using FishTank.Components;
using FishTank.Content;
using FishTank.Models.Levels;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FishTank.Models
{
    public class TopbarItem : Component
    {
        public string Name { get; private set; }

        public Matrix PreTransformMatrix { get; }

        public event EventHandler<PurchaseEventArgs> OnPurchased;

        public LevelItemType ItemType => _model?.Type ?? LevelItemType.Locked;

        public int GoldValue => _model.Cost;

        public TopbarItem(LevelItem item, Rectangle area)
        {
            Area = area.ApplyPadding(Constants.Padding, Alignment.Bottom | Alignment.Left | Alignment.Right | Alignment.Top);
            _model = item;
        }

        public override void LoadContent()
        {
            // Create / preload textures
            ContentBuilder.Instance.CreateRectangleTexture(_textureAssetName, Area.Width, Area.Height);
            ContentBuilder.Instance.LoadFontByName(_fontName);

            switch (ItemType)
            {
                case LevelItemType.PiranhaFish:
                    _iconAssetName = TextureNames.PiranhaAsset;
                    break;
                case LevelItemType.GuppyFish:
                    _iconAssetName = TextureNames.GuppyAsset;
                    break;
                case LevelItemType.Locked:
                    _iconAssetName = TextureNames.BlackLockAsset;
                    break;
            }
        }

        public override void UnloadContent()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D iconAsset = ContentBuilder.Instance.LoadTextureByName(_iconAssetName);
            Texture2D backgroundAsset = ContentBuilder.Instance.LoadTextureByName(_textureAssetName);
            SpriteFont font = ContentBuilder.Instance.LoadFontByName(_fontName);

            spriteBatch.Draw(backgroundAsset, Area.Location.ToVector2(), Color.Red);
            spriteBatch.DrawCenterAt(iconAsset, Area.Center.ToVector2() + new Vector2(0, -20), null);

            if (ItemType != LevelItemType.Locked)
            {
                spriteBatch.DrawString(font, $"{_model.Cost}g", Area, Alignment.Bottom, Color.White);
            }
        }

        public override bool MouseEvent(InputEvent mouseEvent)
        {
            if (ItemType == LevelItemType.Locked)
            {
                return false;
            }

            switch (mouseEvent.Action)
            {
                case InputAction.Click:
                case InputAction.TouchTap:
                    OnPurchased?.Invoke(this, new PurchaseEventArgs(_model));
                    return true;
                case InputAction.Hover:
                case InputAction.HoverExit:
                case InputAction.Release:
                default:
                    break;
            }
            return false;
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            throw new NotImplementedException();
        }

        private LevelItem _model;

        private string _iconAssetName;

        private readonly string _textureAssetName = TextureNames.TopbarItemAsset;

        private readonly string _fontName = FontNames.Arial_20;
    }
}