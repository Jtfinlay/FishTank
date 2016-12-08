//
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
    public class ButtonComponent : IComponent
    {
        public event EventHandler OnClick;

        public Rectangle Area { get; private set; }

        public string ButtonText { get; set; }

        public Matrix PreTransformMatrix { get; private set; }

        public ButtonComponent(Rectangle area, string buttonText)
        {
            Area = area;
            ButtonText = buttonText;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);

            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }
            rect.SetData(data);
            _texture = rect;
            _fishFont = content.Load<SpriteFont>("FishFingers_70");
        }

        public void UnloadContent()
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color backgroundColor = _isMouseOver ? Color.Blue : Color.LightBlue;
            Color fontColor = _isMouseOver ? Color.White : Color.Black;

            spriteBatch.Draw(_texture, Area.Location.ToVector2(), backgroundColor);
            spriteBatch.DrawString(_fishFont, ButtonText, Area, Alignment.Center, fontColor);
        }

        public void Update(GameTime gameTime, MouseState currentMouseState)
        {
            _isMouseOver = Area.Contains(currentMouseState.Position);
        }

        public void MouseEvent(MouseEvent mouseEvent)
        {
            if (mouseEvent.Action == MouseAction.Click)
            {
                OnClick?.Invoke(this, null);
            }
        }

        private bool _isMouseOver;

        private Texture2D _texture;

        private SpriteFont _fishFont;
    }
}
