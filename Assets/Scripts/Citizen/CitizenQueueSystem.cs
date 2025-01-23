using System;
using Queue;
using Shop;

namespace Citizen
{
    
    public class CitizenQueueSystem
    {
        public bool IsVisitedAllShops { get; private set; }
        public ShopData CurrentShop { get; private set; }
        public QueuePoint CurrentQueuePoint { get; private set; }

        private readonly ShopData[] _shops;
        private int _currentShopIndex;
        
        
        public CitizenQueueSystem(ShopData[] shops)
        {
            _shops = shops;
            _currentShopIndex = 0;
            if (_shops.Length > 0)
                CurrentShop = _shops[_currentShopIndex];
        }
        
        public void SetNextShop()
        {
            _currentShopIndex++;
            if (_currentShopIndex < _shops.Length)
            {
                CurrentShop = _shops[_currentShopIndex];
            }
            else
            {
                
                IsVisitedAllShops = true;
            }
        }

        public void TryGetQueuePoint(CitizenController citizen)
        {
            if (CurrentShop != null)
            {
                CurrentQueuePoint = CurrentShop.GetQueuePoint(citizen);
            }
        }

        public void ReleaseQueuePoint()
        {
            if (CurrentShop != null && CurrentQueuePoint != null)
            {
                CurrentShop.ReleaseQueuePoint(CurrentQueuePoint);
                CurrentQueuePoint = null;
            }
        }

        public bool IsQueuePointFree => CurrentQueuePoint == null;
        
        public void StartTrade(Action onTradeComplete)
        {
            if (CurrentShop != null)
            {
                CurrentShop.StartTrade(onTradeComplete);
            }
        }
    }
}