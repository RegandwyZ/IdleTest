using UnityEngine;
using UnityEngine.UI;


namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private Button _upgradeIncome;
        [SerializeField] private Button _upgradeTradeTime;

        private ShopData _shopData;
        private void Awake()
        {
            _shopData = GetComponent<ShopData>();
            
            _upgradeIncome.onClick.AddListener(UpgradeIncome);
            _upgradeTradeTime.onClick.AddListener(UpgradeTradeTime);
        }

        private void UpgradeIncome()
        {
            _shopData.UpgradeIncome();
        }

        private void UpgradeTradeTime()
        {
            _shopData.UpgradeTradeTime();
        }
    }
}