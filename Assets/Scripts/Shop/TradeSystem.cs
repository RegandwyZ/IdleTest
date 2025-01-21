using System;
using System.Collections;
using UnityEngine;

namespace Shop
{
    public class TradeSystem : MonoBehaviour
    {
        private TradeView _tradeView;
        
        private float _tradeTime = 1f;
        private bool _isTradeRunning = false;

        private void Start()
        {
            _tradeView = GetComponent<TradeView>();
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
            _tradeView.ShowProgress(_tradeTime);
            yield return new WaitForSeconds(_tradeTime);
            _isTradeRunning = false;

            
            onTradeComplete?.Invoke();
        }
    }
}