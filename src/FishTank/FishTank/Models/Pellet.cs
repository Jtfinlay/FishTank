//
// Copyright - James Finlay
// 

using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace FishTank.Models
{
    public class Pellet : IInteractable
    {
        public InteractableState State { get; private set; }

        public Rectangle Area { get; private set; }

        private Texture2D _texture;

        private GraphicsDevice _device;

        private const float _fallSpeed = 1.5f;

        public Pellet(GraphicsDevice graphicsDevice, Vector2 position)
        {
            _device = graphicsDevice;
            Area = new Rectangle(position.ToPoint(), new Point(10, 10));
            var rect = new Texture2D(graphicsDevice, Area.Width, Area.Height);

            Color[] data = new Color[Area.Width * Area.Height];
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

            spriteBatch.Draw(_texture, Area.Location.ToVector2(), null);
        }

        public void Update(List<IInteractable> models)
        {
            var position = Vector2.Add(Area.Location.ToVector2(), new Vector2(0, _fallSpeed));
            Area = new Rectangle(position.ToPoint(), Area.Size);

            if (Area.Bottom >= Constants.VirtualHeight)
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
