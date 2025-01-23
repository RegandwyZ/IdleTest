using PlayerCurrentProgress;
using SaveSystem;
using UnityEngine;

namespace EntryPoint
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CurrentProgress _currentProgress;
        [SerializeField] private SaveGameSystem _saveGameSystem;
        [SerializeField] private BuildSystem _buildSystem;
        [SerializeField] private SpawnCitizenSystem _spawnCitizenSystem;
        
        private void Awake()
        {
            _spawnCitizenSystem.Initialize();
            _currentProgress.InitializeCurrentGameData();
            _saveGameSystem.InitializeSaveGameSystem();
            _buildSystem.InitializeShopConfigs();
            
        }
    }
}