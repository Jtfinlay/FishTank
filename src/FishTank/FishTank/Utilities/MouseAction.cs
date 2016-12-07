//
// Copyright - James Finlay
// 

namespace FishTank.Utilities
{
    /// <summary>
    /// Used by <see cref="MouseEvent"/> to identiy mouse action taken on target element.
    /// </summary>
    public enum MouseAction
    {
        /// <summary>
        /// Left button was pressed
        /// </summary>
        Click,

        /// <summary>
        /// Left button was released
        /// </summary>
        Release,

        /// <summary>
        /// Mouse is hovering over target element.
        /// </summary>
        Hover,

        /// <summary>
        /// Mouse has started hovering over target element
        /// </summary>
        HoverStart,

        /// <summary>
        /// Mouse was previously hovering over target element and was just moved away.
        /// </summary>
        HoverExit,
    }
}
