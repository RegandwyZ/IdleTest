using System;
using UnityEngine;


namespace Character.States
{
    public class MoveToPointState : ICharacterState
    {
        private readonly UnityEngine.CharacterController _controller;
        private readonly CharacterMovement _movement;
        private readonly CharacterAnimator _animator;

        
        private readonly Func<Vector3> _getTargetPosition;
        private readonly float _stopDistance;
        private readonly Action _onDestinationReached;

        public MoveToPointState(
            UnityEngine.CharacterController controller,
            CharacterMovement movement,
            CharacterAnimator animator,
            Func<Vector3> getTargetPosition,
            float stopDistance,
            Action onDestinationReached)
        {
            _controller = controller;
            _movement = movement;
            _animator = animator;
            _getTargetPosition = getTargetPosition;
            _stopDistance = stopDistance;
            _onDestinationReached = onDestinationReached;
        }

        public void Enter()
        {
            _animator.SetMoveAnimation();
        }

        public void Update()
        {
            
            bool reached = _movement.MoveTowards(_getTargetPosition(), _stopDistance);
            if (reached)
            {
                _onDestinationReached?.Invoke();
            }
        }

        public void Exit()
        {
            _animator.SetIdleAnimation();
        }
    }
}