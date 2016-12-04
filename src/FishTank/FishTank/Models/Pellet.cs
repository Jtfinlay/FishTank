//
// Copyright - James Finlay
// 

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Models
{
    public class Pellet : IInteractable
    {
        public InteractableState State { get; private set; }

        public Rectangle BoundaryBox { get; private set; }

        private Texture2D _texture;

        private GraphicsDevice _device;

        private const float _fallSpeed = 1.5f;

        public Pellet(GraphicsDevice graphicsDevice, Vector2 position)
        {
            _device = graphicsDevice;
            BoundaryBox = new Rectangle(position.ToPoint(), new Point(10, 10));
            var rect = new Texture2D(graphicsDevice, BoundaryBox.Width, BoundaryBox.Height);

            Color[] data = new Color[BoundaryBox.Width * BoundaryBox.Height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Orange;
            }
            rect.SetData(data);

            _texture = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == InteractableState.Dead)
            {
                // pellet is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(_texture, BoundaryBox.Location.ToVector2(), null);
        }

        public void Update(List<IInteractable> models)
        {
            var position = Vector2.Add(BoundaryBox.Location.ToVector2(), new Vector2(0, _fallSpeed));
            BoundaryBox = new Rectangle(position.ToPoint(), BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                this.State = InteractableState.Dead;
            }
        }

        public void Kill()
        {
            State = InteractableState.Dead;
        }
    }
}
