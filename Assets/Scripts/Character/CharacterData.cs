using Queue;
using Shop;
using UnityEngine;

namespace Character
{
    public class CharacterData : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 0.4f;
        [SerializeField] private float _rotationSpeed = 5f;
        
        [SerializeField] private CharacterAnimator _animator;

        private CharacterState _currentState;

        private Transform[] _points;
        private ShopData[] _shops;
        private ShopData _currentShop;
        
        private QueuePoint _queuePoint;

        private int _currentPointIndex;
        private int _currentShopIndex;

        public void SetData(ShopData[] shopData)
        {
            _animator.SetMoveAnimation();
            _shops = shopData;

            _currentShopIndex = 0; 
            _currentState = CharacterState.MoveToMarketPlace;
        }

        public void SetPathToMarketPlace(Path path)
        {
            _points = path.GetWayPoints();
            _currentPointIndex = 0;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case CharacterState.Idle:
                    break;

                case CharacterState.MoveToMarketPlace:
                    MoveToMarketPlace();
                    break;

                case CharacterState.ToShop:
                    MoveToShop(_currentShop);
                    break;

                case CharacterState.ToQueue:
                    MoveToShopQueue();
                   // _currentState = CharacterState.Idle;
                    break;

                case CharacterState.Trading:
                    _currentShop.StartTrade(() =>
                    {
                        SetNextShop();
                        MoveToNextShop();
                    });
                    break;

                case CharacterState.MoveToTrain:
                    MoveToTrain();
                    break;
            }
        }

        private void MoveToTrain()
        {
           
        }

        private void MoveToShop(ShopData shopData)
        {
            if (_currentState != CharacterState.ToShop) return;

            Vector3 direction = shopData.GetShopPoint().transform.position - transform.position;

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
                shopData.GetShopPoint().transform.position,
                _moveSpeed * Time.deltaTime
            );

            float distanceToPoint = direction.magnitude;
            if (distanceToPoint < 0.01f)
            {
                _animator.SetIdleAnimation();
                _queuePoint = shopData.GetQueuePoint();
                
                _currentState = CharacterState.ToQueue;
            }
        }

        private void MoveToMarketPlace()
        {
            if (_currentState != CharacterState.MoveToMarketPlace) return;
            Vector3 direction = _points[_currentPointIndex].position - transform.position;

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
                _points[_currentPointIndex].position,
                _moveSpeed * Time.deltaTime
            );

            float distanceToPoint = direction.magnitude;
            if (distanceToPoint < 0.01f)
            {
                _currentPointIndex++;

                if (_currentPointIndex >= _points.Length)
                {
                    SetNextShop();
                    _currentState = CharacterState.ToShop;
                }
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
                Debug.Log("All shops visited!");
                _currentState = CharacterState.Idle; 
            }
        }

        private void MoveToNextShop()
        {
            if (_currentShopIndex < _shops.Length)
            {
                _currentState = CharacterState.ToShop;
            }
            else
            {
                Debug.Log("All shops visited!");
                _currentState = CharacterState.Idle;
            }
        }

        private void MoveToShopQueue()
        {
            Vector3 direction = _queuePoint.transform.position - transform.position;

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
                _queuePoint.transform.position,
                _moveSpeed * Time.deltaTime
            );

            float distanceToPoint = direction.magnitude;
            if (distanceToPoint < 0.01f)
            {
                _animator.SetIdleAnimation();
                if (_queuePoint.IsTradePoint)
                {
                    _currentState = CharacterState.Trading;
                }
               
            }
        }
    }
}