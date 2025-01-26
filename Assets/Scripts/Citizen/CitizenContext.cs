using Queue;
using Shop;
using UnityEngine;

namespace Citizen
{
    public class CitizenContext
    {
        public CitizenController Controller;
        
        public Transform[] PointsToTown;
        public Transform[] PointsToTrain;
        public Transform[] PointsToShop;
        
        public ShopData[] Shops;
        public ShopData CurrentShop;
        
        public QueuePoint QueuePoint;
        
        public int CurrentPointToTownIndex;
        public int CurrentPointToTrainIndex;
        public int CurrentPointToShopIndex;
        
        public int CurrentShopIndex;
        
        public bool IsVisitedAllShops;
        
        public Vector3 CenterPoint;
        
        public Vector3 TargetTrainNearPoint;
    }
}