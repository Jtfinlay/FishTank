//
//  Copyright 2017 James Finlay
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 

using Microsoft.Xna.Framework;

namespace FishTank.Utilities
{
    public static class Constants
    {
        public const int VirtualTotalHeight = 1080;

        public const int VirtualTotalWidth = 1920;

        public const int VirtualBarHeight = 175;

        public const int TopBarItems = 9;

        public const int VirtualHeight = VirtualTotalHeight - VirtualBarHeight;

        public const int VirtualWidth = VirtualTotalWidth;

        public static readonly Rectangle VirtualArea = new Rectangle(0, 0, VirtualTotalWidth, VirtualTotalHeight);

        public const float ExpectedFramesPerSecond = 60f;

        public const float ExpectedMillisecondsPerFrame = 1000f / ExpectedFramesPerSecond;

        public const float FallSpeed = 1.5f;

        public const int Padding = 10;
    }
}
