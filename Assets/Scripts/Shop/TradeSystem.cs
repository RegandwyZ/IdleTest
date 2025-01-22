using System;
using System.Collections;
using UnityEngine;

namespace Shop
{
    public class TradeSystem : MonoBehaviour
    {
        [SerializeField] private int _income;
        [SerializeField] private float _tradeTime;
        [SerializeField] private TradeView _tradeView;
        
        private bool _isTradeRunning = false;
        

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
            _tradeView.ShowProgress(_tradeTime);
            yield return new WaitForSeconds(_tradeTime);
            ResourcesSystem.Instance.AddMoney(_income);
            _isTradeRunning = false;
            
            onTradeComplete?.Invoke();
        }

        public void DecreaseTradeTime()
        {
            _tradeTime -= 0.05f;
        }

        public void IncreaseIncome()
        {
            _income += 7;
        }
    }
}