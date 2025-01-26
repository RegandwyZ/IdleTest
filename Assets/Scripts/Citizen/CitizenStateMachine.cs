using PathSystem;
using Queue;
using Shop;
using UnityEngine;

namespace Citizen
{
    public class CitizenStateMachine : MonoBehaviour
    {
        private CitizenController _controller;
        private CitizenMovement _movement;

        private CitizenState _currentState;

        private Transform[] _pointsToShop;
        private Transform[] _pointsToTown;
        private Transform[] _pointsToTrain;

        private ShopData[] _shops;
        private ShopData _currentShop;

        private QueuePoint _queuePoint;

        private int _currentPointToShopIndex;
        private int _currentPointToTownIndex;
        private int _currentPointToTrainIndex;
        private int _currentShopIndex;

        private bool _isVisitedAllShops;

        private Vector3 _centerPoint;
        private Vector3 _targetTrainNearPoint;

        public void Init(CitizenController controller)
        {
            _controller = controller;
            _movement = controller.Movement;
        }

        public void SetData(ShopData[] shopData, Vector3 centerPoint)
        {
            _shops = shopData;
            
            var randomX = Random.Range(-0.5f, 0.5f);
            var randomZ = Random.Range(-0.5f, 0.5f);

            _centerPoint = centerPoint + new Vector3(randomX, 0, randomZ);

            _currentShopIndex = 0;
            _currentState = CitizenState.MoveToMarketPlace;
        }

        public void SetPathTo(CitizenPath citizenPathToMarket, CitizenPath citizenPathToTrain)
        {
            _pointsToTown = citizenPathToMarket.GetWayPoints();
            _pointsToTrain = citizenPathToTrain.GetWayPoints();

            _currentPointToTownIndex = 0;
            _currentPointToTrainIndex = 0;
            
            _controller.Animator.SetSpeedAnimation(_controller.MoveSpeed);
        }

        public void UpdateState()
        {
            switch (_currentState)
            {
                case CitizenState.Idle:
                    _controller.Animator.SetIdleAnimation();
                    break;

                case CitizenState.MoveToMarketPlace:
                    MoveToMarketPlace();
                    break;

                case CitizenState.ToShop:
                    MoveToShop(_currentShop);
                    break;

                case CitizenState.ToQueue:
                    MoveToShopQueue();
                    break;

                case CitizenState.Trading:
                    _controller.Animator.SetIdleAnimation();
                    _currentShop.StartTrade(OnTradeComplete);
                    break;

                case CitizenState.MoveToCenterPoint:
                    MoveToCenterPoint();
                    break;

                case CitizenState.MoveToTrain:
                    MoveToTrain();
                    break;

                case CitizenState.MoveToTrainPlacePoint:
                    MoveToPlaceNearTrain();
                    break;
            }
        }

        private void MoveToMarketPlace()
        {
            if (_movement.MoveTo(_pointsToTown[_currentPointToTownIndex].position))
            {
                _currentPointToTownIndex++;
                if (_currentPointToTownIndex >= _pointsToTown.Length)
                {
                    if (_currentShopIndex < _shops.Length)
                    {
                        SetNextShop();
                        _currentState = CitizenState.ToShop;
                    }
                    else
                    {
                        _currentState = CitizenState.MoveToTrain;
                    }
                }
            }
        }

        private void MoveToShop(ShopData shopData)
        {
            _pointsToShop = shopData.GetShopPoints();

            if (_movement.MoveTo(_pointsToShop[_currentPointToShopIndex].position))
            {
                _currentPointToShopIndex++;
                
                if (_currentPointToShopIndex >= _pointsToShop.Length)
                {
                    _queuePoint = shopData.GetQueuePoint(_controller);
                    if (_queuePoint == null)
                    {
                        _controller.SmileyController.ShowSmiley();
                        SetNextShop();
                        _currentState = CitizenState.MoveToCenterPoint;
                    }
                    else
                    {
                        _currentState = CitizenState.ToQueue;
                    }
                }
            }
        }

        private void MoveToShopQueue()
        {
            if (_movement.MoveTo(_queuePoint.transform.position))
            {
                if (_queuePoint.IsTradePoint)
                {
                    _currentState = CitizenState.Trading;
                }
                else
                {
                    _currentState = CitizenState.Idle;
                    _controller.Animator.SetIdleAnimation();
                }
            }
        }

        private void MoveToCenterPoint()
        {
            if (_currentPointToShopIndex <= 0)
            {
                if (_movement.MoveTo(_centerPoint))
                {
                    _currentState = _isVisitedAllShops ? CitizenState.MoveToTrain : CitizenState.ToShop;
                }
            }
            else
            {
                if (_movement.MoveTo(_pointsToShop[_currentPointToShopIndex - 1].position))
                {
                    _currentPointToShopIndex--;
                }
            }
        }

        private void MoveToTrain()
        {
            if (_currentPointToTrainIndex >= _pointsToTrain.Length)
                return;
            
            var baseTrainPoint = _pointsToTrain[^1].position;
            var randomX = Random.Range(-2f, -10f);
            var randomZ = Random.Range(-1f, 1f);
            
            _targetTrainNearPoint = baseTrainPoint + new Vector3(randomX, 0, randomZ);
            
            if (_movement.MoveTo(_pointsToTrain[_currentPointToTrainIndex].position))
            {
                _currentPointToTrainIndex++;
                
                if (_currentPointToTrainIndex >= _pointsToTrain.Length)
                {
                    _currentState = CitizenState.MoveToTrainPlacePoint;
                }
            }
        }

        private void MoveToPlaceNearTrain()
        {
            if (_movement.MoveTo(_targetTrainNearPoint))
            {
                _currentState = CitizenState.Idle;
                _controller.Animator.SetIdleAnimation();
                _controller.IsReadyToLeave = true;
            }
        }

        private void SetNextShop()
        {
            if (_currentShopIndex < _shops.Length)
            {
                _currentShop = _shops[_currentShopIndex];
                _currentShopIndex++;
            }
            else
            {
                _isVisitedAllShops = true;
            }
        }

        private void OnTradeComplete()
        {
            _currentShop.ReleaseQueuePoint(_queuePoint);
            _queuePoint = null;
            
            SetNextShop();
            _currentState = CitizenState.MoveToCenterPoint;
        }

        public void SetQueuePoint(QueuePoint newQueuePoint)
        {
            _queuePoint = newQueuePoint;
            
            if (_currentState is CitizenState.Idle or CitizenState.ToQueue)
            {
                _currentState = CitizenState.ToQueue;
            }
        }
    }
}