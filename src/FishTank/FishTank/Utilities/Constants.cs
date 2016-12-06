//
// Copyright - James Finlay
// 

namespace FishTank.Utilities
{
    public static class Constants
    {
        public const int VirtualTotalHeight = VirtualBarHeight + VirtualHeight;

        public const int VirtualTotalWidth = VirtualWidth;

        public const int VirtualBarHeight = 100;

        public const int VirtualHeight = 800;

        public const int VirtualWidth = 1300;

        public const float ExpectedFramesPerSecond = 60f;

        public const float ExpectedMillisecondsPerFrame = 1000f / ExpectedFramesPerSecond;

        public const float FallSpeed = 1.5f;
    }
}
