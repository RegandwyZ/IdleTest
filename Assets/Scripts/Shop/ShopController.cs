using PlayerCurrentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopType _shopType;
        [SerializeField] private Button _upgradeIncome;
        [SerializeField] private Button _upgradeTradeTime;

        [SerializeField] private int _costUpIncome;
        [SerializeField] private TextMeshProUGUI _costIncomeText;
        
        [SerializeField] private int _costTradeTime;  
        [SerializeField] private TextMeshProUGUI _costTradeTimeText;

        [SerializeField] private int _incomeMultiplier;
        [SerializeField] private int _tradeMultiplier;

        private int _incomeCount = 1;
        private const int INCOME_COUNT_BLOCK = 15;

        private int _tradeCount = 1;
        private const int TRADE_COUNT_BLOCK = 10;

        private ShopData _shopData;

        private void Start()
        {
            _shopData = GetComponent<ShopData>();

            _upgradeIncome.onClick.AddListener(UpgradeIncome);
            _upgradeTradeTime.onClick.AddListener(UpgradeTradeTime);

            LoadIncomeLevelFromJs();
            LoadTradeTimeFromJs();

            UpdateUI(); 
        }

        private void LoadTradeTimeFromJs()
        {
            var building = CurrentProgress.Instance.CurrentGameData.Buildings
                .Find(b => b.BuildingId == _shopType);

            if (building != null)
            {
                var level = building.TradeTimeLevel;
                for (int i = 1; i < level; i++)
                {
                    LoadTradeTimeFromJson();
                    UpdateUI();
                }
            }
        }

        private void LoadIncomeLevelFromJs()
        {
            var building = CurrentProgress.Instance.CurrentGameData.Buildings
                .Find(b => b.BuildingId == _shopType);

            if (building != null)
            {
                var level = building.IncomeLevel;
                for (int i = 1; i < level; i++)
                {
                    LoadIncomeFromJSon();
                    UpdateUI();
                }
            }
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
                
                CurrentProgress.Instance.UpgradeBuilding(_shopType, ShopUpgradeType.IncreaseMoney);
            }
        }

        private void LoadIncomeFromJSon()
        {
            _shopData.UpgradeIncome();

            _costUpIncome += _incomeMultiplier;
            _incomeMultiplier += 15;
            
            _incomeCount++;
        }

        private void LoadTradeTimeFromJson()
        {
            _shopData.UpgradeTradeTime();

            _costTradeTime += _tradeMultiplier;
            _tradeMultiplier += 9;
            
            _tradeCount++;
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
                
                CurrentProgress.Instance.UpgradeBuilding(_shopType, ShopUpgradeType.DecreaseTradeTime);
            }
        }

        private void UpdateUI()
        {
            _costIncomeText.text = $"{_costUpIncome}";
            _costTradeTimeText.text = $"{_costTradeTime}";
        }
    }
}