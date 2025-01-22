using System;
using Queue;
using Shop;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        public bool IsReadyToLeave { get; private set; }
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        [SerializeField] private CharacterAnimator _animator;
        
        private CharacterState _currentState;

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
            _shops = shopData;
            float randomX = Random.Range(-6f, 6f);
            float randomZ = Random.Range(-6f, 6f);
            
            _centerPoint = centerPoint + new Vector3(randomX, 0, randomZ);
            _currentShopIndex = 0;
            _currentState = CharacterState.MoveToMarketPlace;
        }

        public void SetPathToMarketPlace(CitizenPath citizenPath)
        {
            _pointsToMarket = citizenPath.GetWayPoints();
            _currentPointToMarketIndex = 0;
        }

        public void SetPathToTrainPlace(CitizenPath citizenPath)
        {
            _pointsToTrain = citizenPath.GetWayPoints();
            _currentPointToTrainIndex = 0;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case CharacterState.Idle:
                    _animator.SetIdleAnimation();
                    break;

                case CharacterState.MoveToMarketPlace:
                    MoveToMarketPlace();
                    break;

                case CharacterState.ToShop:
                    MoveToShop(_currentShop);
                    break;

                case CharacterState.ToQueue:
                    MoveToShopQueue();
                    break;

                case CharacterState.Trading:
                    _animator.SetIdleAnimation();
                    _currentShop.StartTrade(OnTradeComplete);
                    break;
                
                case CharacterState.MoveToCenterPoint:
                    MoveToCenterPoint();
                    break;

                case CharacterState.MoveToTrain:
                    MoveToTrain();
                    break;
                
                case CharacterState.MoveToTrainPlacePoint:
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
                _currentPointToTrainIndex + 1 >= _pointsToTrain.Length ? CharacterState.MoveToTrainPlacePoint : _currentState,
                () =>
                {
                    _currentPointToTrainIndex++;
                    if (_currentPointToTrainIndex >= _pointsToTrain.Length)
                    {
                        _currentState = CharacterState.MoveToTrainPlacePoint;
                    }
                }
            );
        }

        private void MoveToPlaceNearTrain()
        {
            MoveTo(
                _targetTrainNearPoint,
                0.5f,
                CharacterState.Idle,
                () =>
                {
                    _currentState = CharacterState.Idle;
                    _animator.SetIdleAnimation();
                    IsReadyToLeave = true;
                }
            );
        }

        private void MoveTo(Vector3 targetPosition, float stopDistance, CharacterState nextState,
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
                CharacterState.Idle,
                () =>
                {
                    _queuePoint = shopData.GetQueuePoint(this);

                    if (_queuePoint == null)
                    {
                        SetNextShop();
                        
                        _currentState = CharacterState.MoveToCenterPoint;
                    }
                    else
                    {
                        _currentState = CharacterState.ToQueue;
                    }
                }
            );
        }
        
        private void MoveToShopQueue()
        {
            MoveTo(
                _queuePoint.transform.position,
                0.01f,
                _queuePoint.IsTradePoint ? CharacterState.Trading : CharacterState.Idle,
                () =>
                {
                    if (!_queuePoint.IsTradePoint)
                    {
                        _animator.SetIdleAnimation();
                    }
                }
            );
        }
        
        private void OnTradeComplete()
        {
            _currentShop.ReleaseQueuePoint(_queuePoint);
            _queuePoint = null;
            
            SetNextShop();
            _currentState = CharacterState.MoveToCenterPoint;
        }

        private void MoveToCenterPoint()
        {
            var state = _isVisitedAllShops ? CharacterState.MoveToTrain : CharacterState.ToShop;
            
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
                    ? CharacterState.Idle
                    : _currentState,
                () =>
                {
                    _currentPointToMarketIndex++;

                    if (_currentPointToMarketIndex >= _pointsToMarket.Length)
                    {
                        if (_currentShopIndex < _shops.Length)
                        {
                            SetNextShop();
                            _currentState = CharacterState.ToShop;
                        }
                        else
                        {
                            Debug.Log("Все магазины пройдены!");
                            _currentState = CharacterState.MoveToTrain;
                        }
                    }
                }
            );
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
                Debug.Log("Все магазины пройдены!");
                _isVisitedAllShops = true;
            }
        }
        
        public void SetQueuePoint(QueuePoint newQueuePoint)
        {
            _queuePoint = newQueuePoint;
            if (_currentState == CharacterState.Idle || _currentState == CharacterState.ToQueue)
            {
                _currentState = CharacterState.ToQueue;
            }
        }
    }
}