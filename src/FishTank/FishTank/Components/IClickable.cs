//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Components
{
    public interface IClickable
    {
        Rectangle Area { get; }

        void MouseHover(MouseState mouseState);

        void MouseClick(MouseState mouseState);

        void MouseRelease(MouseState mouseState);
    }
}
