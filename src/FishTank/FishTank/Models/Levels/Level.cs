//
// Copyright - James Finlay
// 

namespace FishTank.Models.Levels
{
    public abstract class Level
    {
        public LevelItem[] Items { get; protected set; } = new LevelItem[8];
    }
}
