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

namespace FishTank.Drawing
{
    public static class SpriteSheets
    {
        public static readonly SpriteSheet ClownFishSheet = new SpriteSheet("sheets\\clownfish_sheet.png", new Point(70, 60));

        public static readonly SpriteSheet CoinSheet = new SpriteSheet("sheets\\coin_sheet.png", new Point(30, 30));

        public static readonly SpriteSheet CrabSheet = new SpriteSheet("sheets\\crab_sheet.png", new Point(140, 100));

        public static readonly SpriteSheet GuppyFishSheet = new SpriteSheet("sheets\\guppy_sheet.png", new Point(47, 40));

        public static readonly SpriteSheet KingFishSheet = new SpriteSheet("sheets\\kingfish_sheet.png", new Point(70, 60));

        public static readonly SpriteSheet PiranhaSheet = new SpriteSheet("sheets\\piranha_sheet.png", new Point(120, 88));
    }
}
