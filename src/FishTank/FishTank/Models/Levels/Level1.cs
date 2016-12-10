//
// Copyright - James Finlay
// 

namespace FishTank.Models.Levels
{
    public class Level1 : Level
    {
        public Level1()
        {
            WorldId = 1;
            LevelId = 1;
            Items[0] = new LevelItem(LevelItemTypes.GuppyFish, 100);
        }
    }
}
