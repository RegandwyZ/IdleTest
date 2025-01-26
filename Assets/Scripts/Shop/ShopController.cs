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
        
        [SerializeField] private TextMeshProUGUI _costIncomeText;
        [SerializeField] private TextMeshProUGUI _costTradeTimeText;
        
        [SerializeField] private int _initialCostIncome;
        [SerializeField] private int _initialCostTradeTime;
        [SerializeField] private int _incomeMultiplierIncrement;
        [SerializeField] private int _tradeMultiplierIncrement;
        [SerializeField] private int _incomeUpgradeLimit;
        [SerializeField] private int _tradeUpgradeLimit;

        private ShopData _shopData;

        private int _currentCostIncome;
        private int _currentCostTradeTime;
        private int _currentIncomeMultiplier;
        private int _currentTradeMultiplier;
        private int _incomeCount = 1;
        private int _tradeCount = 1;

        private void Start()
        {
            _shopData = GetComponent<ShopData>();

            InitializeUpgrades();
            LoadProgressFromJson();
            UpdateUI();
        }

        private void InitializeUpgrades()
        {
            _currentCostIncome = _initialCostIncome;
            _currentCostTradeTime = _initialCostTradeTime;
            _currentIncomeMultiplier = _incomeMultiplierIncrement;
            _currentTradeMultiplier = _tradeMultiplierIncrement;

            _upgradeIncome.onClick.AddListener(() => Upgrade(ShopUpgradeType.IncreaseMoney, ref _incomeCount, ref _currentCostIncome, ref _currentIncomeMultiplier, _incomeUpgradeLimit, _shopData.UpgradeIncome));
            _upgradeTradeTime.onClick.AddListener(() => Upgrade(ShopUpgradeType.DecreaseTradeTime, ref _tradeCount, ref _currentCostTradeTime, ref _currentTradeMultiplier, _tradeUpgradeLimit, _shopData.UpgradeTradeTime));
        }

        private void LoadProgressFromJson()
        {
            var building = CurrentProgress.Instance.CurrentGameData.Buildings.Find(b => b.BuildingId == _shopType);
            if (building != null)
            {
                LoadUpgradesFromJson(building.IncomeLevel, ref _incomeCount, ref _currentCostIncome, ref _currentIncomeMultiplier, _shopData.UpgradeIncome);
                LoadUpgradesFromJson(building.TradeTimeLevel, ref _tradeCount, ref _currentCostTradeTime, ref _currentTradeMultiplier, _shopData.UpgradeTradeTime);
            }
        }

        private void LoadUpgradesFromJson(int level, ref int count, ref int currentCost, ref int multiplier, System.Action upgradeAction)
        {
            for (int i = 1; i < level; i++)
            {
                ApplyUpgrade(ref count, ref currentCost, ref multiplier, upgradeAction);
            }
        }

        private void Upgrade(ShopUpgradeType upgradeType, ref int count, ref int currentCost, ref int multiplier, int upgradeLimit, System.Action upgradeAction)
        {
            if (count >= upgradeLimit || !ResourcesSystem.Instance.SpendMoney(currentCost)) return;

            ApplyUpgrade(ref count, ref currentCost, ref multiplier, upgradeAction);
            CurrentProgress.Instance.UpgradeBuilding(_shopType, upgradeType);

            if (count >= upgradeLimit)
            {
                DisableButton(upgradeType);
            }
        }

        private void ApplyUpgrade(ref int count, ref int currentCost, ref int multiplier, System.Action upgradeAction)
        {
            upgradeAction.Invoke();
            currentCost += multiplier;
            multiplier += 2;
            count++;
            UpdateUI();
        }

        private void DisableButton(ShopUpgradeType upgradeType)
        {
            if (upgradeType == ShopUpgradeType.IncreaseMoney)
                _upgradeIncome.interactable = false;
            else if (upgradeType == ShopUpgradeType.DecreaseTradeTime)
                _upgradeTradeTime.interactable = false;
        }

        private void UpdateUI()
        {
            _costIncomeText.text = $"{_currentCostIncome}";
            _costTradeTimeText.text = $"{_currentCostTradeTime}";
        }
    }
}