using UnityEngine;

namespace PathSystem
{
    public class CitizenPath : MonoBehaviour
    {
        [SerializeField] private Transform[] _wayPoints;

        public Transform[] GetWayPoints()
        {
            return _wayPoints;
        }
    }
}
