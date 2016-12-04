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
        public Vector2 Position { get; private set; }

        public InteractableState State { get; private set; }

        private Texture2D _texture;

        private GraphicsDevice _device;

        private const float _fallSpeed = 2f;

        public Pellet(GraphicsDevice graphicsDevice, Vector2 position)
        {
            _device = graphicsDevice;
            var rect = new Texture2D(graphicsDevice, 10, 10);

            Color[] data = new Color[10 * 10];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Orange;
            }
            rect.SetData(data);

            _texture = rect;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == InteractableState.Dead)
            {
                // pellet is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(_texture, Position, null);
        }

        public void Update(List<IInteractable> models)
        {
            Position = Vector2.Add(Position, new Vector2(0, _fallSpeed));

            if (Position.Y >= Constants.VirtualHeight)
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
