using System;
using Queue;

namespace Character.States
{
    public class QueueState : ICharacterState
    {
        private readonly CharacterController _controller;
        private readonly CharacterMovement _movement;
        private readonly CharacterAnimator _animator;
        private readonly QueuePoint _queuePoint;
        private readonly Action _onQueuePositionReached;
        
        private bool _hasReachedQueuePosition;

        public QueueState(
            CharacterController controller,
            CharacterMovement movement,
            CharacterAnimator animator,
            QueuePoint queuePoint,
            Action onQueuePositionReached)
        {
            _controller = controller;
            _movement = movement;
            _animator = animator;
            _queuePoint = queuePoint;
            _onQueuePositionReached = onQueuePositionReached;
        }

        public void Enter()
        {
            _animator.SetMoveAnimation();
            _hasReachedQueuePosition = false;
        }

        public void Update()
        {
            if (!_hasReachedQueuePosition)
            {
                bool reached = _movement.MoveTowards(_queuePoint.transform.position, 0.01f);
                if (reached)
                {
                    _hasReachedQueuePosition = true;
                    _animator.SetIdleAnimation();
                    _onQueuePositionReached?.Invoke();
                }
            }
        }

        public void Exit()
        {
            
        }
    }
}