using System;
using PathSystem;
using Queue;
using Shop;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Citizen
{
    public class CitizenController : MonoBehaviour
    {
        public bool IsReadyToLeave { get; private set; }
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        [SerializeField] private CitizenAnimator _animator;
        
        private CitizenState _currentState;
        private CharacterMovementSystem _movementSystem;
        
        private Transform[] _pointsToMarket;
        private Transform[] _pointsToTrain;
        private ShopData[] _shops;
        private ShopData _currentShop;

        private QueuePoint _queuePoint;

        private int _currentPointToMarketIndex;
        private int _currentPointToTrainIndex;
        private int _currentShopIndex;
        private bool _isVisitedAllShops;
        private Vector3 _centerPoint;
        private Vector3 _targetTrainNearPoint;
        private Vector3 _previousPosition;
        
        
        public void SetData(ShopData[] shopData, Vector3 centerPoint)
        {
            _movementSystem = new CharacterMovementSystem(transform, _animator, _moveSpeed, _rotationSpeed, _pointsToMarket, _pointsToTrain );
            _shops = shopData;
            float randomX = Random.Range(-6f, 6f);
            float randomZ = Random.Range(-6f, 6f);
            
            _centerPoint = centerPoint + new Vector3(randomX, 0, randomZ);
            _currentShopIndex = 0;
            _currentState = CitizenState.MoveToMarketPlace;
        }

        public void SetPathTo(CitizenPath citizenPathToMarket, CitizenPath citizenPathToTrain)
        {
            _pointsToMarket = citizenPathToMarket.GetWayPoints();
            _pointsToTrain = citizenPathToTrain.GetWayPoints(); 
            
            _currentPointToTrainIndex = 0;
            _currentPointToMarketIndex = 0;
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
                
            float randomX = Random.Range(-2f, 10f);
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
            MoveTo(
                shopData.GetShopPoint().position,
                0.01f,
                CitizenState.Idle,
                () =>
                {
                    _queuePoint = shopData.GetQueuePoint(this);

                    if (_queuePoint == null)
                    {
                        SetNextShop();
                        _currentState = CitizenState.MoveToCenterPoint;
                    }
                    else
                    {
                        _currentState = CitizenState.ToQueue;
                    }
                }
            );
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
        
       

        private void MoveToCenterPoint()
        {
            var state = _isVisitedAllShops ? CitizenState.MoveToTrain : CitizenState.ToShop;
            
            MoveTo(
                _centerPoint,
                0.01f,
                state
            );
        }

        private void MoveToMarketPlace()
        {
            MoveTo(
                _pointsToMarket[_currentPointToMarketIndex].position,
                0.01f,
                _currentPointToMarketIndex + 1 >= _pointsToMarket.Length
                    ? CitizenState.Idle
                    : _currentState,
                () =>
                {
                    _currentPointToMarketIndex++;

                    if (_currentPointToMarketIndex >= _pointsToMarket.Length)
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