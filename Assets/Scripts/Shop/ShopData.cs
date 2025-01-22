using System;
using Queue;
using UnityEngine;
using CharacterController = Character.CharacterController;

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

        public void UpgradeTradeTime()
        {
            _tradeSystem.DecreaseTradeTime();
        }

        public void UpgradeIncome()
        {
            _tradeSystem.IncreaseIncome();
        }
        public Transform GetShopPoint()
        {
            return _shopPoint;
        }
        
        public QueuePoint GetQueuePoint(CharacterController occupant)
        {
            foreach (var point in _queuePoints)
            {
                if (point.CurrentState == QueueState.Empty)
                {
                    point.AssignOccupant(occupant);
                    return point;
                }
            }
            
            return null;
        }
        
        public void StartTrade(Action onTradeComplete)
        {
            _tradeSystem.Trade(onTradeComplete);
        }

        public void ReleaseQueuePoint(QueuePoint freedPoint)
        {
            freedPoint.ClearOccupant();
            
            for (int i = 0; i < _queuePoints.Length - 1; i++)
            {
                if (_queuePoints[i].CurrentState == QueueState.Empty &&
                    _queuePoints[i + 1].CurrentState == QueueState.Engaged)
                {
                    var nextOccupant = _queuePoints[i + 1].Occupant;

                    _queuePoints[i].AssignOccupant(nextOccupant);

                    nextOccupant.SetQueuePoint(_queuePoints[i]);
                    
                    _queuePoints[i + 1].ClearOccupant();
                }
            }
        }
    }
}