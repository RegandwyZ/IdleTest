using PlayerCurrentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bridge
{
    public class BridgeController : MonoBehaviour
    {
        [SerializeField] private BridgeData _bridgeData;
        [SerializeField] private int _bridgeCost;
        [SerializeField] private TextMeshProUGUI _bridgeCostText;
        [SerializeField] private Button _bridgeButton;
        [SerializeField] private Canvas _bridgeCanvas;
        [SerializeField] private CurrentProgress _currentProgress;
        [SerializeField] private GameObject _maskNorthBridge;
        public void BridgeInitialize()
        {
            if (_currentProgress.CurrentGameData.NorthBridge)
            {
                _bridgeData.gameObject.SetActive(true);
                _maskNorthBridge.SetActive(false);
                _bridgeCanvas.gameObject.SetActive(false);
            }
            else
            {
                _bridgeCanvas.gameObject.SetActive(true);
                _bridgeButton.onClick.AddListener(ConstructBridge);
                _bridgeCostText.text = $"${_bridgeCost}";
            }
       
        }

        private void ConstructBridge()
        {
            if (ResourcesSystem.Instance.SpendMoney(_bridgeCost))
            {
                _currentProgress.AddNorthBridge();
                _bridgeData.gameObject.SetActive(true);
                _bridgeCanvas.gameObject.SetActive(false);
                _maskNorthBridge.SetActive(false);
            }
        
        }
    }
}
