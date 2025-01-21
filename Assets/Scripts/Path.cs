using UnityEngine;


public class Path : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;

    public Transform[] GetWayPoints()
    {
        return _wayPoints;
    }
}
