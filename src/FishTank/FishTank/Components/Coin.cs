//
// Copyright - James Finlay
// 

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using FishTank.Utilities.Inputs;
using System;
using FishTank.Content;

namespace FishTank.Components
{
    public class Coin : IInteractable, IClickable
    {
        public event EventHandler OnClick;
        public Rectangle BoundaryBox => Area;

        public InteractableState State { get; private set; }

        public Rectangle Area { get; private set; }

        public int GoldValue { get; private set; } = 125;

        public Coin(GraphicsDevice graphicsDevice, Vector2 position)
        {
            Area = new Rectangle(position.ToPoint(), new Point(20, 20));
            ContentBuilder.Instance.CreateRectangleTexture(TextureName, BoundaryBox.Width, BoundaryBox.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == InteractableState.Discard)
            {
                // coin is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(TextureName), BoundaryBox.Location.ToVector2(), Color.Gold);
        }

        public void Update(List<IInteractable> models, GameTime gameTime)
        {
            var position = Vector2.Add(BoundaryBox.Location.ToVector2(), new Vector2(0, Constants.FallSpeed));
            Area = new Rectangle(position.ToPoint(), BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                State = InteractableState.Discard;
            }
        }

        public bool MouseEvent(MouseEvent mouseEvent)
        {
            if (mouseEvent.Action == MouseAction.Click)
            {
                OnClick?.Invoke(this, null);
                State = InteractableState.Discard;
                return true;
            }
            return false;
        }

        public readonly string TextureName = "CoinRectangleAsset";
    }
}
