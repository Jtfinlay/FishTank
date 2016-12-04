//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Utilities
{
    public static class Extensions
    {
        public static bool IsWithinVirtualBounds(this Vector2 mouse)
        {
            return mouse.X < Constants.VirtualTotalWidth && mouse.X > 0
                && mouse.Y < Constants.VirtualTotalHeight && mouse.Y > 0;
        }

        public static MouseState SetPosition(this MouseState mouseState, Vector2 position)
        {
            return new MouseState((int)position.X, (int)position.Y, mouseState.ScrollWheelValue,
                mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton,
                mouseState.XButton1, mouseState.XButton2);
        }
    }
}
