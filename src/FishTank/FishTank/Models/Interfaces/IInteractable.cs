//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Models.Interfaces
{
    public interface IInteractable
    {
        void Update(List<IInteractable> models);

        void Draw(SpriteBatch spriteBatch);

        Vector2 Position { get; }

        InteractableState State { get; }
    }
}
