﻿//
// Copyright - James Finlay
// 

namespace FishTank.Utilities
{
    public static class Constants
    {
        public const int VirtualTotalHeight = 1080;

        public const int VirtualTotalWidth = 1920;

        public const int VirtualBarHeight = 100;

        public const int VirtualHeight = VirtualTotalHeight - VirtualBarHeight;

        public const int VirtualWidth = VirtualTotalWidth;

        public const float ExpectedFramesPerSecond = 60f;

        public const float ExpectedMillisecondsPerFrame = 1000f / ExpectedFramesPerSecond;

        public const float FallSpeed = 1.5f;
    }
}
