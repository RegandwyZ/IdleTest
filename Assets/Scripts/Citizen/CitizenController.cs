using System;
using PathSystem;
using Queue;
using Shop;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Citizen
{
    public class CitizenController : MonoBehaviour
    {
        public bool IsReadyToLeave { get; private set; }
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private SmileyController _smileyController;
        [SerializeField] private CitizenAnimator _animator;
        
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
        private Vector3 _previousPosition;
        
        
        public void SetData(ShopData[] shopData, Vector3 centerPoint)
        {
            _shops = shopData;
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            
            _centerPoint = centerPoint + new Vector3(randomX, 0, randomZ);
            _currentShopIndex = 0;
            _currentState = CitizenState.MoveToMarketPlace;
        }

        public void SetPathTo(CitizenPath citizenPathToMarket, CitizenPath citizenPathToTrain)
        {
            _pointsToTown = citizenPathToMarket.GetWayPoints();
            _pointsToTrain = citizenPathToTrain.GetWayPoints(); 
            
            _currentPointToTrainIndex = 0;
            _currentPointToTownIndex = 0;
        }
        
        private void Start()
        {
            _animator.SetSpeedAnimation(_moveSpeed);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case CitizenState.Idle:
                    _animator.SetIdleAnimation();
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
                    _animator.SetIdleAnimation();
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

        private void MoveToTrain()
        {
            if (_currentPointToTrainIndex >= _pointsToTrain.Length)
            {
                return;
            }
            
            Vector3 baseTrainPoint = _pointsToTrain[^1].position;
                
            float randomX = Random.Range(-2f, -10f);
            float randomZ = Random.Range(-1f, 1f);

            _targetTrainNearPoint = baseTrainPoint + new Vector3(randomX, 0, randomZ);
            MoveTo(
                _pointsToTrain[_currentPointToTrainIndex].position,
                0.01f,
                _currentPointToTrainIndex + 1 >= _pointsToTrain.Length ? CitizenState.MoveToTrainPlacePoint : _currentState,
                () =>
                {
                    _currentPointToTrainIndex++;
                    if (_currentPointToTrainIndex >= _pointsToTrain.Length)
                    {
                        _currentState = CitizenState.MoveToTrainPlacePoint;
                    }
                }
            );
        }

        private void MoveToPlaceNearTrain()
        {
            MoveTo(
                _targetTrainNearPoint,
                0.5f,
                CitizenState.Idle,
                () =>
                {
                    _currentState = CitizenState.Idle;
                    _animator.SetIdleAnimation();
                    IsReadyToLeave = true;
                }
            );
        }

        private void MoveTo(Vector3 targetPosition, float stopDistance, CitizenState nextState,
            Action onReachTarget = null)
        {
            _animator.SetMoveAnimation();
            
            Vector3 direction = targetPosition - transform.position;

            if (direction.sqrMagnitude > 0.0001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime
            );

            if (direction.magnitude < stopDistance)
            {
                _animator.SetIdleAnimation();
                _currentState = nextState;
                
                onReachTarget?.Invoke();
            }
        }
        
        private void MoveToShop(ShopData shopData)
        {
            _pointsToShop = shopData.GetShopPoints();
            MoveTo(
                _pointsToShop[_currentPointToShopIndex].position,
                0.01f,
                _currentPointToShopIndex + 1 >= _pointsToShop.Length
                    ? CitizenState.Idle
                    : _currentState,
                () =>
                {
                    _currentPointToShopIndex++;

                    if (_currentPointToShopIndex >= _pointsToShop.Length)
                    {
                        _queuePoint = shopData.GetQueuePoint(this);

                        if (_queuePoint == null)
                        {
                            SetNextShop();
                            _smileyController.ShowSmiley();
                            _currentState = CitizenState.MoveToCenterPoint;
                        }
                        else
                        {
                            _currentState = CitizenState.ToQueue;
                        }
                    }
                }
            );
        }
        

        private void MoveToCenterPoint()
        {
            if (_currentPointToShopIndex <= 0)
            {
                MoveTo(
                    _centerPoint,
                    0.01f,
                    _isVisitedAllShops ? CitizenState.MoveToTrain : CitizenState.ToShop
                );
            }
            else
            {
                MoveTo(
                    _pointsToShop[_currentPointToShopIndex - 1].position,
                    0.01f,
                    CitizenState.MoveToCenterPoint,
                    () =>
                    {
                        _currentPointToShopIndex--;
                    }
                );
            }
        }

        private void MoveToMarketPlace()
        {
            MoveTo(
                _pointsToTown[_currentPointToTownIndex].position,
                0.01f,
                _currentPointToTownIndex + 1 >= _pointsToTown.Length
                    ? CitizenState.Idle
                    : _currentState,
                () =>
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
            );
        }
        
        private void OnTradeComplete()
        {
            _currentShop.ReleaseQueuePoint(_queuePoint);
            _queuePoint = null;
            
            SetNextShop();
            _currentState = CitizenState.MoveToCenterPoint;
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
        
        private void MoveToShopQueue()
        {
            MoveTo(
                _queuePoint.transform.position,
                0.01f,
                _queuePoint.IsTradePoint ? CitizenState.Trading : CitizenState.Idle,
                () =>
                {
                    if (!_queuePoint.IsTradePoint)
                    {
                        _animator.SetIdleAnimation();
                    }
                }
            );
        }
        
        public void SetQueuePoint(QueuePoint newQueuePoint)
        {
            _queuePoint = newQueuePoint;
            if (_currentState == CitizenState.Idle || _currentState == CitizenState.ToQueue)
            {
                _currentState = CitizenState.ToQueue;
            }
        }
    }
}