using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private Button _upgradeIncome;
        [SerializeField] private Button _upgradeTradeTime;

        [SerializeField] private int _costUpIncome;
        [SerializeField] private TextMeshProUGUI _costIncomeText;
        
        [SerializeField] private int _costTradeTime;  
        [SerializeField] private TextMeshProUGUI _costTradeTimeText;

        [SerializeField] private int _incomeMultiplier;
        [SerializeField] private int _tradeMultiplier;

        private int _incomeCount = 0;
        private const int INCOME_COUNT_BLOCK = 15;

        private int _tradeCount = 0;
        private const int TRADE_COUNT_BLOCK = 10;

        private ShopData _shopData;

        private void Awake()
        {
            _shopData = GetComponent<ShopData>();

            _upgradeIncome.onClick.AddListener(UpgradeIncome);
            _upgradeTradeTime.onClick.AddListener(UpgradeTradeTime);

            UpdateUI(); 
        }

        private void UpgradeIncome()
        {
            if (_incomeCount >= INCOME_COUNT_BLOCK)
            {
                return;
            }

            if (ResourcesSystem.Instance.SpendMoney(_costUpIncome))
            {
                _shopData.UpgradeIncome();

                _costUpIncome += _incomeMultiplier;
                _incomeMultiplier += 15;

                UpdateUI();
                _incomeCount++;
            }
        }

        private void UpgradeTradeTime()
        {
            
            if (_tradeCount >= TRADE_COUNT_BLOCK)
            {
                return;
            }

            if (ResourcesSystem.Instance.SpendMoney(_costTradeTime))
            {
                _shopData.UpgradeTradeTime();

                _costTradeTime += _tradeMultiplier;
                _tradeMultiplier += 9;

                UpdateUI();
                _tradeCount++;
            }
        }

        private void UpdateUI()
        {
            _costIncomeText.text = $"{_costUpIncome}";
            _costTradeTimeText.text = $"{_costTradeTime}";
        }
    }
}