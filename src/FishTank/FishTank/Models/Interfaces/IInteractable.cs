//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Models.Interfaces
{
    /// <summary>
    /// Interface used to represent all interactable objects in the game space.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Allows interactable to run logic that allows for movement, collision
        /// checking, and more.
        /// </summary>
        /// <param name="models"></param>
        void Update(List<IInteractable> models);

        /// <summary>
        /// Called when the interactable should draw itself
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Rectangle indicating the boundary box for the interactable
        /// </summary>
        Rectangle BoundaryBox { get; }

        /// <summary>
        /// State of the object indicating whether interactable with other objects
        /// </summary>
        InteractableState State { get; }
    }
}
