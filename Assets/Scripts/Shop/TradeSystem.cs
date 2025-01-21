using System;
using System.Collections;
using UnityEngine;

namespace Shop
{
    public class TradeSystem : MonoBehaviour
    {
        private float _tradeTime = 5f;
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
            yield return new WaitForSeconds(_tradeTime);
            Debug.Log("Trade complete");

            
            onTradeComplete?.Invoke();
        }
    }
}