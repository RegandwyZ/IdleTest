using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class AllPathHolder : MonoBehaviour
    {
        public CitizenPath CitizenPathToMarket => _citizenPathToMarket;
        [FormerlySerializedAs("_pathToMarket")] [SerializeField] private CitizenPath _citizenPathToMarket;
        
        public CitizenPath CitizenPathToTrain => _citizenPathToTrain;
        [FormerlySerializedAs("_pathToTrain")] [SerializeField] private CitizenPath _citizenPathToTrain;
    }
}