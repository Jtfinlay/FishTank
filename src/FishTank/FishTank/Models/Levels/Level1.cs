//
// Copyright - James Finlay
// 

namespace FishTank.Models.Levels
{
    public class Level1 : Level
    {
        public Level1()
        {
            Items[0] = new LevelItem(LevelItemTypes.GuppyFish, 100);
        }
    }
}
