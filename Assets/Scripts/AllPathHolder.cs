using UnityEngine;

namespace DefaultNamespace
{
    public class AllPathHolder : MonoBehaviour
    {
        public Path PathToMarket => _pathToMarket;
        [SerializeField] private Path _pathToMarket;
        
        public Path PathToTrain => _pathToTrain;
        [SerializeField] private Path _pathToTrain;
    }
}