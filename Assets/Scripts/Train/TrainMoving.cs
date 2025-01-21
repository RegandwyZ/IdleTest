using System.Collections;
using UnityEngine;

namespace Train
{
    public class TrainMoving : MonoBehaviour
    {
        [SerializeField] private Transform _pointA; 
        [SerializeField] private Transform _pointB; 
        [SerializeField] SpawnNewWayCharacter _spawnNewWayCharacter;
        
        private readonly float _speed = 45f; 

        private void Start()
        {
            StartCoroutine(MoveTrain());
        }

        private IEnumerator MoveTrain()
        {
            while (true)
            {
                yield return StartCoroutine(MoveToPoint(_pointB.position));
                _spawnNewWayCharacter.SpawnNewWay();
                yield return new WaitForSeconds(0.7f);
            
                yield return StartCoroutine(MoveToPoint(_pointA.position));
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator MoveToPoint(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
                yield return null; 
            }
        
            transform.position = target;
        }
    }
}
