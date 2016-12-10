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

namespace FishTank.Components
{
    public class Coin : IInteractable, IClickable
    {
        public event EventHandler OnClick;
        public Rectangle BoundaryBox => Area;

        public InteractableState State { get; private set; }

        public Rectangle Area { get; private set; }

        public Coin(GraphicsDevice graphicsDevice, Vector2 position)
        {
            Area = new Rectangle(position.ToPoint(), new Point(20, 20));
            var rect = new Texture2D(graphicsDevice, BoundaryBox.Width, BoundaryBox.Height);

            Color[] data = new Color[BoundaryBox.Width * BoundaryBox.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Gold;
            }
            rect.SetData(data);

            _texture = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == InteractableState.Discard)
            {
                // pellet is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(_texture, BoundaryBox.Location.ToVector2(), null);
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

        private Texture2D _texture;
    }
}
