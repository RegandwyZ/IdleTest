using UnityEngine;

namespace Citizen
{
    public class CitizenAnimator : MonoBehaviour
    {
        private static readonly int move = Animator.StringToHash("IsMoving");
        private static readonly int speed = Animator.StringToHash("Speed");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetIdleAnimation()
        {
            _animator.SetBool(move, false);
        }

        public void SetMoveAnimation()
        {
            _animator.SetBool(move, true);
        }

        public void SetSpeedAnimation(float moveSpeed)
        {
            _animator.SetFloat(speed, moveSpeed);
        }
    }
}