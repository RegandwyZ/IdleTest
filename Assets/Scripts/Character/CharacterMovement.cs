using UnityEngine;

namespace Character
{
    public class CharacterMovement
    {
        private readonly Transform _characterTransform;
        private readonly float _moveSpeed;
        private readonly float _rotationSpeed;

        public CharacterMovement(Transform characterTransform, float moveSpeed, float rotationSpeed)
        {
            _characterTransform = characterTransform;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
        }

        public bool MoveTowards(Vector3 targetPosition, float stopDistance)
        {
            Vector3 direction = targetPosition - _characterTransform.position;
            if (direction.sqrMagnitude < stopDistance * stopDistance)
            {
                return true; 
            }
            
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

            return false; 
        }
    }
}