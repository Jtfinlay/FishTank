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

using System;

namespace FishTank.Models.Levels
{
    public class LevelItem
    {
        public event EventHandler OnPurchase;

        public LevelItemType Type { get; private set; }

        public int CurrentLevel { get; private set; } = 1;

        public int MaxLevel { get; private set; } = 1;// todo - remove default.

        public int Cost { get; private set; }

        public LevelItem(LevelItemType type, int cost)
        {
            Type = type;
            Cost = cost;
        }

        public void Purchase()
        {
            OnPurchase?.Invoke(this, null);
        }
    }
}
