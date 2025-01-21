namespace Character.States
{
    public class IdleState : ICharacterState
    {
        private readonly CharacterController _controller;
        private readonly CharacterMovement _movement;
        private readonly CharacterAnimator _animator;

        public IdleState(CharacterController controller, CharacterMovement movement, CharacterAnimator animator)
        {
            _controller = controller;
            _movement = movement;
            _animator = animator;
        }

        public void Enter()
        {
            _animator.SetIdleAnimation();
        }

        public void Update()
        {
           
        }

        public void Exit()
        {
            
        }
    
    }
}