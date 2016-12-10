//
// Copyright - James Finlay
// 

using FishTank.Components;
using FishTank.Models.Levels;
using FishTank.Utilities;
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

        public event EventHandler OnPurchased;

        public LevelItemTypes ItemType { get; private set; }

        public int GoldValue { get; private set; } = 100;

        public TopbarItem(LevelItem item, Rectangle area)
        {
            Area = area;
            ItemType = item?.Type ?? LevelItemTypes.Locked;
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            var rect = new Texture2D(graphicsDevice, Area.Width - _padding * 2, Area.Height - _padding * 2);

            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Red;
            }
            rect.SetData(data);
            _texture = rect;
            _fishFont = content.Load<SpriteFont>("Arial_20");
            switch (ItemType)
            {
                case LevelItemTypes.GuppyFish:
                    _icon = content.Load<Texture2D>("Guppy.png");
                    break;
                case LevelItemTypes.Locked:
                    _icon = content.Load<Texture2D>("BlackLock_Small.png");
                    break;
            }
        }

        public override void UnloadContent()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Area.Location.ToVector2() + new Vector2(_padding, _padding), null);
            spriteBatch.DrawCenterAt(_icon, Area.Center.ToVector2() + new Vector2(0, -20), null);

            if (ItemType != LevelItemTypes.Locked)
            {
                spriteBatch.DrawString(_fishFont, "100g", Area, Alignment.Bottom, Color.White);
            }
        }

        public override bool MouseEvent(MouseEvent mouseEvent)
        {
            switch (mouseEvent.Action)
            {
                case MouseAction.Click:
                    OnPurchased?.Invoke(this, new EventArgs());
                    return true;
                case MouseAction.Hover:
                case MouseAction.HoverExit:
                case MouseAction.Release:
                default:
                    break;
            }
            return false;
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
            throw new NotImplementedException();
        }

        private Texture2D _texture;
        private Texture2D _icon;
        private SpriteFont _fishFont;

        private const int _padding = 10;
    }
}