using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Utilities.Inputs
{
    /// <summary>
    /// Class to identify the mouse actions performed on an object
    /// </summary>
    public class InputEvent
    {
        /// <summary>
        /// Location where the mouse event occurred
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Location where the mouse event occurred
        /// </summary>
        public Vector2 Location => Position.ToVector2();

        /// <summary>
        /// The action performed on the target element
        /// </summary>
        public InputAction Action { get; private set; }


        public InputEvent(MouseState state, InputAction action)
        {
            Action = action;
            Position = state.Position;
            _state = state;
        }

        public InputEvent(InputAction action, Point position)
        {
            Action = action;
            Position = position;
        }

        /// <summary>
        /// Overall state of the mouse instance
        /// </summary>
        private MouseState _state { get; set; }
    }
}
