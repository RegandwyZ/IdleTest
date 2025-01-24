using System.Collections.Generic;
using SaveSystem;
using Shop;
using UnityEngine;

namespace PlayerCurrentProgress
{
    public class CurrentProgress : MonoBehaviour
    {
        public static CurrentProgress Instance { get; private set; }
        public GameData CurrentGameData;

        
        public void InitializeCurrentGameData()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            if (CurrentGameData == null)
            {
                CurrentGameData = new GameData
                {
                    Buildings = new List<BuildingData>()
                };
            }
        }
        

        public void ChangeMoney(int amount)
        {
            if (CurrentGameData == null) return;
            CurrentGameData.Money += amount;
        }

       
        public void AddBuilding(ShopType buildingId)
        {
            if (CurrentGameData?.Buildings == null) return;
            if (CurrentGameData.Buildings.Exists(b => b.BuildingId == buildingId))
                return;

            CurrentGameData.Buildings.Add(new BuildingData
            {
                BuildingId = buildingId,
                IncomeLevel = 1,
                TradeTimeLevel = 1
            });
           
        }

        public void AddNorthBridge()
        {
            if (CurrentGameData == null) return;
            CurrentGameData.NorthBridge = true;
        }

        
        public void UpgradeBuilding(ShopType buildingId, ShopUpgradeType upgradeType)
        {
            if (CurrentGameData == null || CurrentGameData.Buildings == null) return;

            BuildingData building = CurrentGameData.Buildings.Find(b => b.BuildingId == buildingId);
            if (building != null)
            {
                switch (upgradeType)
                {
                    case ShopUpgradeType.IncreaseMoney:
                        building.IncomeLevel++;
                        break;
                    
                    case ShopUpgradeType.DecreaseTradeTime:
                        building.TradeTimeLevel++;
                        break;
                   
                }
            }
        }
    }
}