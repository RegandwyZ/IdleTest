using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Citizen
{
    public class CharacterMovementSystem
    {
        private readonly Transform _characterTransform;
        private readonly CitizenAnimator _animator;
        
        private readonly float _moveSpeed;
        private readonly float _rotationSpeed;
        
        private readonly Transform[] _pointsToMarket;
        private int _currentPointToMarketIndex;

       
        private readonly Transform[] _pointsToTrain;
        private int _currentPointToTrainIndex;
        
        
        private Vector3 _targetTrainNearPoint;

        public CharacterMovementSystem(
            Transform characterTransform,
            CitizenAnimator animator,
            float moveSpeed,
            float rotationSpeed,
            Transform[] pointsToMarket,
            Transform[] pointsToTrain)
        {
            _characterTransform = characterTransform;
            _animator = animator;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;

            _pointsToMarket = pointsToMarket;
            _pointsToTrain = pointsToTrain;

            _currentPointToMarketIndex = 0;
            _currentPointToTrainIndex = 0;
        }
        
        public void MoveToMarketPlace(Action onReachedLastPoint = null)
        {
            if (_pointsToMarket == null || _pointsToMarket.Length == 0)
                return;
            
            MoveTo(
                _pointsToMarket[_currentPointToMarketIndex].position,
                0.01f,
                () =>
                {
                    _currentPointToMarketIndex++;
                    if (_currentPointToMarketIndex >= _pointsToMarket.Length)
                    {
                        onReachedLastPoint?.Invoke();
                    }
                }
            );
        }

        
        public void MoveToTrain(Action onReachedAllPoints = null)
        {
            if (_pointsToTrain == null || _pointsToTrain.Length == 0)
                return;

            if (_currentPointToTrainIndex < _pointsToTrain.Length)
            {
                if (_currentPointToTrainIndex == _pointsToTrain.Length - 1)
                {
                    Vector3 baseTrainPoint = _pointsToTrain[_currentPointToTrainIndex].position;
                    float randomX = Random.Range(-2f, 10f);
                    float randomZ = Random.Range(-1f, 1f);
                    _targetTrainNearPoint = baseTrainPoint + new Vector3(randomX, 0, randomZ);
                }

                MoveTo(
                    _pointsToTrain[_currentPointToTrainIndex].position,
                    0.01f,
                    () =>
                    {
                        _currentPointToTrainIndex++;
                        if (_currentPointToTrainIndex >= _pointsToTrain.Length)
                        {
                            onReachedAllPoints?.Invoke();
                        }
                    }
                );
            }
        }
        
        public void MoveToTrainNearPoint(Action onReachedPoint = null)
        {
            MoveTo(
                _targetTrainNearPoint,
                0.5f,
                onReachedPoint
            );
        }
        
        private void MoveTo(Vector3 targetPosition, float stopDistance, Action onReachTarget = null)
        {
            _animator.SetMoveAnimation();

            Vector3 direction = targetPosition - _characterTransform.position;
            
            if (direction.sqrMagnitude > 0.0001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _characterTransform.rotation = Quaternion.Slerp(
                    _characterTransform.rotation,
                    lookRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }
            
            _characterTransform.position = Vector3.MoveTowards(
                _characterTransform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime
            );
            
            if (direction.magnitude < stopDistance)
            {
                
                _animator.SetIdleAnimation();
                onReachTarget?.Invoke();
            }
        }
        
        public void MoveToPoint(Vector3 targetPosition, float stopDistance, Action onReached = null)
        {
            MoveTo(targetPosition, stopDistance, onReached);
        }
    }
}