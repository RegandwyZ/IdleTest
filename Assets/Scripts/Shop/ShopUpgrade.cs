using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class ShopUpgrade: MonoBehaviour
    {
        [SerializeField] private ShopData _gardeningShop;
        [SerializeField] private ShopData _fashionShop;
        [SerializeField] private ShopData _pawnShop;
        [SerializeField] private ShopData _magicShop;
        
        private Dictionary<ShopType, ShopData> _shopDictionary;

        private void Awake()
        {
            _shopDictionary = new Dictionary<ShopType, ShopData>
            {
                { ShopType.Gardening, _gardeningShop },
                { ShopType.Fashion, _fashionShop },
                { ShopType.Pawn, _pawnShop },
                { ShopType.Magic, _magicShop }
            };
        }

        public void UpgradeIncome(ShopType shopType)
        {
            if (_shopDictionary.TryGetValue(shopType, out var shopData))
            {
                shopData.UpgradeIncome();
            }
        }

        public void UpgradeTradeTime(ShopType shopType)
        {
            if (_shopDictionary.TryGetValue(shopType, out var shopData))
            {
                shopData.UpgradeTradeTime();
            }
        }
    }
}