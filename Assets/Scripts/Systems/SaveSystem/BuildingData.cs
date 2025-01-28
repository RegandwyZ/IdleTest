using System;
using Shop;

namespace Systems.SaveSystem
{
    [Serializable]
    public class BuildingData 
    {
        public ShopType BuildingId;    
        public int IncomeLevel;        
        public int TradeTimeLevel;
    }
}
