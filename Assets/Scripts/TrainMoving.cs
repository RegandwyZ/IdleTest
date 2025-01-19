using System.Collections;
using UnityEngine;

public class TrainMoving : MonoBehaviour
{
   [SerializeField] private Transform _pointA; 
   [SerializeField] private Transform _pointB; 
    
    private readonly float _speed = 15f; 

    private void Start()
    {
        StartCoroutine(MoveTrain());
    }

    private IEnumerator MoveTrain()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToPoint(_pointB.position));
            yield return new WaitForSeconds(7);
            
            yield return StartCoroutine(MoveToPoint(_pointA.position));
            yield return new WaitForSeconds(5);
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
