using System;
using Queue;
using UnityEngine;

namespace Shop
{
    public class ShopData : MonoBehaviour
    {
        [SerializeField] private Transform _shopPoint;
        [SerializeField] private QueuePoint[] _queuePoints;
        
        private TradeSystem _tradeSystem;

        private void Start()
        {
            _queuePoints[0].SetTradePoint();
            _tradeSystem = GetComponent<TradeSystem>();
        }

        public Transform GetShopPoint()
        {
            return _shopPoint;
        }
        
        public QueuePoint GetQueuePoint()
        {
            foreach (var point in _queuePoints)
            {
                if (point.CurrentState == QueueState.Empty)
                {
                    point.ChangeState(QueueState.Engaged);
                    return point;
                }
            }


            return null;
        }

        public void StartTrade(Action onTradeComplete)
        {
            _tradeSystem.Trade(onTradeComplete);
        }
    }
}