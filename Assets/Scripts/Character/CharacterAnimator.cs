using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Move = Animator.StringToHash("Move");
        
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
    }
}