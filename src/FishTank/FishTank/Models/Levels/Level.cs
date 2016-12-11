//
//  Copyright 2016 James Finlay
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

using FishTank.Utilities;

namespace FishTank.Models.Levels
{
    public abstract class Level
    {
        public LevelItem[] Items { get; protected set; } = new LevelItem[Constants.TopBarItems];

        public int WorldId { get; protected set; }

        public int LevelId { get; protected set; }

        public int InitialGold { get; protected set; }

        public string LevelName => $"Level {WorldId} - {LevelId}";
    }
}
