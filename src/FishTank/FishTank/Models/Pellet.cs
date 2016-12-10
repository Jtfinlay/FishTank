//
// Copyright - James Finlay
// 

using FishTank.Content;
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

        public Pellet(GraphicsDevice graphicsDevice, Vector2 position)
        {
            BoundaryBox = new Rectangle(position.ToPoint(), new Point(20, 20));
            ContentBuilder.Instance.CreateRectangleTexture(TextureName, BoundaryBox.Width, BoundaryBox.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == InteractableState.Discard)
            {
                // pellet is destroyed but awaiting cleanup. Don't draw
                return;
            }

            spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(TextureName), BoundaryBox.Location.ToVector2(), Color.LightGreen);
        }

        public void Update(List<IInteractable> models, GameTime gameTime)
        {
            var position = Vector2.Add(BoundaryBox.Location.ToVector2(), new Vector2(0, Constants.FallSpeed));
            BoundaryBox = new Rectangle(position.ToPoint(), BoundaryBox.Size);

            if (BoundaryBox.Bottom >= Constants.VirtualHeight)
            {
                this.State = InteractableState.Discard;
            }
        }

        public void Eat()
        {
            State = InteractableState.Discard;
        }

        public readonly string TextureName = "PelletRectangleAsset";
    }
}
