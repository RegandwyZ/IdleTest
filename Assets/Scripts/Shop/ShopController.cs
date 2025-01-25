using System;
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

            _upgradeIncome.onClick.AddListener(() => Upgrade(ShopUpgradeType.IncreaseMoney, ref _incomeCount, ref _costUpIncome, ref _incomeMultiplier, INCOME_COUNT_BLOCK, _shopData.UpgradeIncome));
            _upgradeTradeTime.onClick.AddListener(() => Upgrade(ShopUpgradeType.DecreaseTradeTime, ref _tradeCount, ref _costTradeTime, ref _tradeMultiplier, TRADE_COUNT_BLOCK, _shopData.UpgradeTradeTime));

            LoadLevelFromJs(ShopUpgradeType.IncreaseMoney, ref _incomeCount, ref _costUpIncome, ref _incomeMultiplier, _shopData.UpgradeIncome);
            LoadLevelFromJs(ShopUpgradeType.DecreaseTradeTime, ref _tradeCount, ref _costTradeTime, ref _tradeMultiplier, _shopData.UpgradeTradeTime);

            UpdateUI();
        }

        private void LoadLevelFromJs(ShopUpgradeType upgradeType, ref int count, ref int cost, ref int multiplier, Action upgradeAction)
        {
            var building = CurrentProgress.Instance.CurrentGameData.Buildings
                .Find(b => b.BuildingId == _shopType);

            if (building == null) return;

            int level = upgradeType == ShopUpgradeType.IncreaseMoney ? building.IncomeLevel : building.TradeTimeLevel;

            for (int i = 1; i < level; i++)
            {
                LoadUpgradeFromJson(ref count, ref cost, ref multiplier, upgradeAction);
            }

            UpdateUI();
        }

        private void LoadUpgradeFromJson(ref int count, ref int cost, ref int multiplier, Action upgradeAction)
        {
            upgradeAction.Invoke();

            cost += multiplier;
            multiplier += (upgradeAction == _shopData.UpgradeIncome) ? 15 : 9;

            count++;
        }

        private void Upgrade(ShopUpgradeType upgradeType, ref int count, ref int cost, ref int multiplier, int maxCount, System.Action upgradeAction)
        {
            if (count >= maxCount) return;

            if (ResourcesSystem.Instance.SpendMoney(cost))
            {
                upgradeAction.Invoke();

                cost += multiplier;
                multiplier += (upgradeAction == _shopData.UpgradeIncome) ? 15 : 9;

                count++;

                CurrentProgress.Instance.UpgradeBuilding(_shopType, upgradeType);

                if (count >= maxCount)
                {
                    if (upgradeType == ShopUpgradeType.IncreaseMoney)
                        _upgradeIncome.interactable = false;
                    else
                        _upgradeTradeTime.interactable = false;
                }

                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            _costIncomeText.text = $"{_costUpIncome}";
            _costTradeTimeText.text = $"{_costTradeTime}";
        }
    }
}