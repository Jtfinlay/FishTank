using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Utilities.Inputs
{
    /// <summary>
    /// Class to identify the mouse actions performed on an object
    /// </summary>
    public class MouseEvent
    {
        /// <summary>
        /// Location where the mouse event occurred
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// Location where the mouse event occurred
        /// </summary>
        public Vector2 Location => Position.ToVector2();

        /// <summary>
        /// The action performed on the target element
        /// </summary>
        public MouseAction Action { get; private set; }

        /// <summary>
        /// Overall state of the mouse instance
        /// </summary>
        public MouseState State { get; private set; }

        public MouseEvent(MouseState state, MouseAction action)
        {
            Action = action;
            Position = state.Position;
            State = state;
        }
    }
}
