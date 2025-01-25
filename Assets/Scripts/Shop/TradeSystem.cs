using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Shop
{
    public class TradeSystem : MonoBehaviour
    {
        [SerializeField] private int _income;
        [SerializeField] private float _tradeTime;
        [SerializeField] private TradeView _tradeView;
        [SerializeField] private TextMeshProUGUI _incomeText;
        [SerializeField] private TextMeshProUGUI _tradeTimeText;
        [SerializeField] private int _multiplier;

        private const float TRADE_TIME_DECREASE_TIME = 0.05f;
        
        private int _basicMultiplier;
        private bool _isTradeRunning = false;


        private int Income
        {
            get => _income;
            set
            {
                _income = value;
                UpdateUI();
            }
        }

        private float TradeTime
        {
            get => _tradeTime;
            set
            {
                _tradeTime = value;
                UpdateUI();
            }
        }

        private void Awake()
        {
            UpdateUI();
            _basicMultiplier = _multiplier;
        }

        public void Trade(Action onTradeComplete)
        {
            if (!_isTradeRunning)
            {
                StartCoroutine(StartTradeCoroutine(onTradeComplete));
                _isTradeRunning = true;
            }
        }

        private IEnumerator StartTradeCoroutine(Action onTradeComplete)
        {
            _tradeView.SetMoneyForTrade(_income);
            _tradeView.ShowProgress(_tradeTime);
            yield return new WaitForSeconds(_tradeTime);
            ResourcesSystem.Instance.AddMoney(_income);
            _isTradeRunning = false;

            onTradeComplete?.Invoke();
        }

        public void DecreaseTradeTime()
        {
            TradeTime -= TRADE_TIME_DECREASE_TIME; 
        }

        public void IncreaseIncome()
        {
            Income += _multiplier;
            //_multiplier += _basicMultiplier;
        }

        private void UpdateUI()
        {
            if (_incomeText != null)
                _incomeText.text = $"{_income}";

            if (_tradeTimeText != null)
                _tradeTimeText.text = $"{_tradeTime:F2}";
        }
    }
}