using UnityEngine;


namespace Bridge
{
    public class BridgeUIActivator : MonoBehaviour
    {
        [SerializeField] private GameObject _uiLock;
        [SerializeField] private BridgeData _bridgeData;

        private void OnEnable()
        {
            if (_bridgeData.IsEnabled)
            {
                _uiLock.gameObject.SetActive(false);   
            }
        }

        private void OnDisable()
        {
            if (!_bridgeData.IsEnabled)
            {
                _uiLock.gameObject.SetActive(true);
            }
        }
    }
}