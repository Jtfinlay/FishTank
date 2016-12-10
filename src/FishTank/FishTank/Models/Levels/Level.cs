//
// Copyright - James Finlay
// 

using FishTank.Utilities;

namespace FishTank.Models.Levels
{
    public abstract class Level
    {
        public LevelItem[] Items { get; protected set; } = new LevelItem[Constants.TopBarItems];

        public int WorldId { get; protected set; }

        public int LevelId { get; protected set; }

        public string LevelName => $"Level {WorldId} - {LevelId}";
    }
}
