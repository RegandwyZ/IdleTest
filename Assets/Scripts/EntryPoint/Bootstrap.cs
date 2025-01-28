using Bridge;
using PlayerCurrentProgress;
using Shop;
using Systems.BuildSystem;
using Systems.ResourcesSystem;
using Systems.SaveSystem;
using UnityEngine;


namespace EntryPoint
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CurrentProgress _currentProgress;
        [SerializeField] private SaveGameSystem _saveGameSystem;
        [SerializeField] private BuildSystem _buildSystem;
        [SerializeField] private ActiveShopHolder _activeShops;
        [SerializeField] private ResourcesSystem _resourcesSystem;
        [SerializeField] private BridgeController _bridgeController;
        
        private void Awake()
        {
            _activeShops.Initialize();
            _currentProgress.InitializeCurrentGameData();
            _saveGameSystem.InitializeSaveGameSystem();
            _buildSystem.InitializeShopConfigs();
            _buildSystem.InitializeBridges();
            _resourcesSystem.InitializeResourcesSystem();
            _bridgeController.BridgeInitialize();
        }
    }
}