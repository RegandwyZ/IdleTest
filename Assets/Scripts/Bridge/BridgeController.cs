using PlayerCurrentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bridge
{
    public class BridgeController : MonoBehaviour
    {
        [SerializeField] private global::Bridge.Bridge _bridge;
        [SerializeField] private int _bridgeCost;
        [SerializeField] private TextMeshProUGUI _bridgeCostText;
        [SerializeField] private Button _bridgeButton;
        [SerializeField] private Canvas _bridgeCanvas;
        [SerializeField] private CurrentProgress _currentProgress;
    
        public void BridgeInitialize()
        {
            if (_currentProgress.CurrentGameData.NorthBridge)
            {
                _bridge.gameObject.SetActive(true);
                _bridgeCanvas.gameObject.SetActive(false);
            }
            else
            {
                _bridgeButton.onClick.AddListener(ConstructBridge);
                _bridgeCostText.text = _bridgeCost.ToString();
            }
       
        }

        private void ConstructBridge()
        {
            if (ResourcesSystem.Instance.SpendMoney(_bridgeCost))
            {
                _currentProgress.AddNorthBridge();
                _bridge.gameObject.SetActive(true);
                _bridgeCanvas.gameObject.SetActive(false);
            }
        
        }
    }
}
