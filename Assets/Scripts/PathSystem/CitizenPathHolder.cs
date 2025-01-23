using UnityEngine;

namespace PathSystem
{
    public class CitizenPathHolder : MonoBehaviour
    {
        public CitizenPath CitizenPathToMarket => _citizenPathToMarket;
        [SerializeField] private CitizenPath _citizenPathToMarket;
        
        public CitizenPath CitizenPathToTrain => _citizenPathToTrain;
        [SerializeField] private CitizenPath _citizenPathToTrain;

        public Transform CentralPoint => _centralPoint;
        [SerializeField] private Transform _centralPoint;

    }
}