﻿//
// Copyright - James Finlay
// 

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

        public ButtonComponent(Rectangle area, string buttonText, string fontName = "FishFingers_70")
        {
            Area = area;
            ButtonText = buttonText;
            FontName = fontName;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);

            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }
            rect.SetData(data);
            _texture = rect;
            _fishFont = content.Load<SpriteFont>(FontName);
        }

        public override void UnloadContent()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color backgroundColor = _isMouseOver ? Color.Blue : Color.LightBlue;
            Color fontColor = _isMouseOver ? Color.White : Color.Black;

            spriteBatch.Draw(_texture, Area.Location.ToVector2(), backgroundColor);
            spriteBatch.DrawString(_fishFont, ButtonText, Area, Alignment.Center, fontColor);
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

        private Texture2D _texture;

        private SpriteFont _fishFont;
    }
}
