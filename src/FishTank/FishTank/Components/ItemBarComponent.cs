//
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
    public class ItemBarComponent : Component
    {
        public event EventHandler OnPurchaseFish;

        public ItemBarComponent()
        {
            Area = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualBarHeight);
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            var buyGuppyFish = new TopbarItem(graphicsDevice, new Rectangle(0, 0, Area.Height, Area.Height));
            var upgradeFood = new TopbarItem(graphicsDevice, new Rectangle(Area.Height, 0, Area.Height, Area.Height));
            var upgradeFoodDrop = new TopbarItem(graphicsDevice, new Rectangle(2 * Area.Height, 0, Area.Height, Area.Height));
            var buyPiranhaFish = new TopbarItem(graphicsDevice, new Rectangle(3*Area.Height, 0, Area.Height, Area.Height));
            var buyBlasterUpgrade = new TopbarItem(graphicsDevice, new Rectangle(4*Area.Height, 0, Area.Height, Area.Height));
            var buyEgg = new TopbarItem(graphicsDevice, new Rectangle(5*Area.Height, 0, Area.Height, Area.Height));

            var otherItem1 = new TopbarItem(graphicsDevice, new Rectangle(6 * Area.Height, 0, Area.Height, Area.Height));
            var otherItem2 = new TopbarItem(graphicsDevice, new Rectangle(7 * Area.Height, 0, Area.Height, Area.Height));


            buyGuppyFish.OnClicked += (s,e) => OnPurchaseFish?.Invoke(this, new EventArgs());

            _buttons = new List<TopbarItem>() { buyGuppyFish, upgradeFood, upgradeFoodDrop, buyPiranhaFish, buyBlasterUpgrade, buyEgg, otherItem1, otherItem2};

            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);

            Color[] data = new Color[Area.Width * Area.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Coral;
            }
            rect.SetData(data);
            _texture = rect;
        }

        public override void UnloadContent()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(0, 0), null);
            _buttons.ForEach((button) => button.Draw(spriteBatch));
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState)
        {
        }

        public override void MouseEvent(MouseEvent mouseEvent)
        {
            _buttons.Where((button) => button.Area.Contains(mouseEvent.Position)).FirstOrDefault()?.MouseEvent(mouseEvent);
        }

        private Texture2D _texture;

        private List<TopbarItem> _buttons;

        private Matrix _postScaleTransform = Matrix.CreateTranslation(0,0,0);
    }
}
