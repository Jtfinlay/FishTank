//
// Copyright - James Finlay
// 

using System;

namespace FishTank.Models.Levels
{
    public class LevelItem
    {
        public event EventHandler OnPurchase;

        public LevelItemTypes Type { get; private set; }

        public int CurrentLevel { get; private set; } = 1;

        public int MaxLevel { get; private set; } = 1;// todo - remove default.

        public int Cost { get; private set; }

        public LevelItem(LevelItemTypes type, int cost)
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
