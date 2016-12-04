//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Utilities
{
    public static class Extensions
    {
        public static MouseState SetPosition(this MouseState mouseState, Vector2 position)
        {
            return new MouseState((int)position.X, (int)position.Y, mouseState.ScrollWheelValue,
                mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton,
                mouseState.XButton1, mouseState.XButton2);
        }

        public static bool Within(this Point point, Rectangle area)
        {
            return (point.X > area.Left) && (point.X < area.Right)
                && (point.Y > area.Top) && (point.Y < area.Bottom);
        }
    }
}
