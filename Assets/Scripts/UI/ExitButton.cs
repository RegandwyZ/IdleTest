using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private GameObject _exitPanel;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ClosePanel);
        }

        private void ClosePanel()
        {
            _exitPanel.SetActive(false);
        }
    }
}
