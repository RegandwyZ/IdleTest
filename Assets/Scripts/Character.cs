using System;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.4f;
    [SerializeField] private float _rotationSpeed = 5f;

    private Vector3 _targetPosition;

    private void Start()
    {
        _targetPosition = new Vector3(22, 2, 34);
    }

    private void Update()
    {
        
        Vector3 direction = _targetPosition - transform.position;
        
        if (direction.sqrMagnitude > 0.0001f)
        {
            
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
        
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetPosition,
            _moveSpeed * Time.deltaTime
        );
    }
}
