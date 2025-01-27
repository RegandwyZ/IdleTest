using UnityEngine;

namespace Shop
{
    public class ActiveShopHolder : MonoBehaviour
    {
        [SerializeField] private ShopData[] _startShopData;
        [SerializeField] private BuildSystem _buildSystem;

        public void Initialize()
        {
            _buildSystem.OnShopPurchased += AddShopToArray;
        }

        private void OnDestroy()
        {
            _buildSystem.OnShopPurchased -= AddShopToArray;
        }

        private void AddShopToArray(ShopData newShop)
        {
            var updatedShops = new ShopData[_startShopData.Length + 1];
            for (int i = 0; i < _startShopData.Length; i++)
            {
                updatedShops[i] = _startShopData[i];
            }

            updatedShops[^1] = newShop;
            _startShopData = updatedShops;
        }

        private ShopData[] ShuffleShops(ShopData[] original)
        {
            ShopData[] shuffled = (ShopData[])original.Clone();

            for (int i = shuffled.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
            }

            return shuffled;
        }

        public ShopData[] GetShops()
        {
            return ShuffleShops(_startShopData);
        }
    }
}