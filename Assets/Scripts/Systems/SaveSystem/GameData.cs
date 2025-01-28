using System;
using System.Collections.Generic;

namespace Systems.SaveSystem
{
    [Serializable]
    public class GameData
    {
        public int Money;
        public List<BuildingData> Buildings;
        public bool NorthBridge;
    }
}