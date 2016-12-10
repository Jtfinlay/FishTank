//
// Copyright - James Finlay
// 

using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;

namespace FishTank.Components
{
    /// <summary>
    /// Implementing objects indicate that they expect to receive click events
    /// </summary>
    public interface IClickable
    {
        /// <summary>
        /// The target area of the object. Used by parent to filter whether object was clicked.
        /// </summary>
        Rectangle Area { get; }

        /// <summary>
        /// Invoked when a mouse action was performed on the element
        /// </summary>
        /// <param name="mouseEvent">Contains details of the action performed</param>
        /// <returns>Boolean indicating whether this object has taken action from event</returns>
        bool MouseEvent(MouseEvent mouseEvent);
    }
}
