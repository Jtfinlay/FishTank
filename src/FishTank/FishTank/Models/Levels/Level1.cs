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

namespace FishTank.Models.Levels
{
    public class Level1 : Level
    {
        public Level1()
        {
            WorldId = 1;
            LevelId = 1;
            Items[0] = new LevelItem(LevelItemType.ClownFish, 100);
            Items[2] = new LevelItem(LevelItemType.PiranhaFish, 200);
            InitialGold = int.MaxValue / 1000;
        }
    }
}
