using SoundSystem;
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
            AudioSystem.Instance.PlaySfx(SfxType.ExitBuilding);
            _exitPanel.SetActive(false);
        }
    }
}