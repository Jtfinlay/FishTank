﻿//
// Copyright - James Finlay
// 

using FishTank.Models;
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
    public class ItemBarComponent : IComponent
    {
        public event EventHandler OnPurchaseFish;

        public Matrix PreTransformMatrix => _postScaleTransform;

        public Rectangle Area { get; private set; }

        public ItemBarComponent()
        {
            Area = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualBarHeight);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            var buyGoldFish = new TopbarItem(graphicsDevice, new Rectangle(0, 0, Area.Height, Area.Height));
            buyGoldFish.OnClicked += (s,e) => OnPurchaseFish?.Invoke(this, new EventArgs());

            _buttons = new List<TopbarItem>() { buyGoldFish };

            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);

            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Coral;
            }
            rect.SetData(data);
            _texture = rect;
        }

        public void UnloadContent()
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(0, 0), null);
            _buttons.ForEach((button) => button.Draw(spriteBatch));
        }

        public void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        public void MouseEvent(MouseEvent mouseEvent)
        {
            _buttons.Where((button) => button.Area.Contains(mouseEvent.Position)).FirstOrDefault()?.MouseEvent(mouseEvent);
        }

        private Texture2D _texture;

        private List<TopbarItem> _buttons;

        private Matrix _postScaleTransform = Matrix.CreateTranslation(0,0,0);
    }
}
