using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetIdleAnimation()
        {
            _animator.SetTrigger(Idle);
        }

        public void SetMoveAnimation()
        {
            _animator.SetTrigger(Move);
        }

        public void SetSpeedAnimation(float moveSpeed)
        {
            _animator.SetFloat(Speed, moveSpeed);
        }
    }
}