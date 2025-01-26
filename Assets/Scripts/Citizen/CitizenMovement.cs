using UnityEngine;

namespace Citizen
{
    public class CitizenMovement : MonoBehaviour
    {
        private CitizenController _controller;
        
        private const float STOP_DISTANCE = 0.01f;
        
        public void Init(CitizenController controller)
        {
            _controller = controller;
        }
        
        public bool MoveTo(Vector3 targetPosition)
        {
            _controller.Animator.SetMoveAnimation();

            var direction = targetPosition - transform.position;
            var distance = direction.magnitude;
            
            if (distance > 0.0001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    _controller.RotationSpeed * Time.deltaTime
                );
            }
            
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _controller.MoveSpeed * Time.deltaTime
            );
            
            if (distance < STOP_DISTANCE)
            {
                _controller.Animator.SetIdleAnimation();
                return true;
            }

            return false;
        }
    }
}