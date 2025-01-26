using UnityEngine;

namespace Citizen
{
    public class CitizenAnimator : MonoBehaviour
    {
        
        private static readonly int Move = Animator.StringToHash("IsMoving");
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetIdleAnimation()
        {
            _animator.SetBool(Move, false);
        }

        public void SetMoveAnimation()
        {
            _animator.SetBool(Move, true);
        }

        public void SetSpeedAnimation(float moveSpeed)
        {
            _animator.SetFloat(Speed, moveSpeed);
        }
    }
}