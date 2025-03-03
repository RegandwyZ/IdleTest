﻿using System;
using Citizen;
using Queue;
using UnityEngine;

namespace Shop
{
    public class ShopData : MonoBehaviour
    {
        [SerializeField] private Transform[] _shopPoint;
        [SerializeField] private QueuePoint[] _queuePoints;

        [SerializeField] private TradeSystem _tradeSystem;

        private void Awake()
        {
            _queuePoints[0].SetTradePoint();
        }

        public void UpgradeTradeTime()
        {
            _tradeSystem.DecreaseTradeTime();
        }

        public void UpgradeIncome()
        {
            _tradeSystem.IncreaseIncome();
        }

        public Transform[] GetShopPoints()
        {
            return _shopPoint;
        }

        public QueuePoint GetQueuePoint(CitizenController occupant)
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