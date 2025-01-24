using UnityEngine;

namespace Bridge
{
    public class BridgeUIActivator : MonoBehaviour
    {
        [SerializeField] private GameObject _uiLock;
        [SerializeField] private Bridge _bridge;

        private void OnEnable()
        {
            if (_bridge.IsEnabled)
            {
                _uiLock.gameObject.SetActive(false);   
            }
        }

        private void OnDisable()
        {
            if (!_bridge.IsEnabled)
            {
                _uiLock.gameObject.SetActive(true);
            }
        }
    }
}