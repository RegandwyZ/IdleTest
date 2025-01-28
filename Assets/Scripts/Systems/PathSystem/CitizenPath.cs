using UnityEngine;

namespace Systems.PathSystem
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
