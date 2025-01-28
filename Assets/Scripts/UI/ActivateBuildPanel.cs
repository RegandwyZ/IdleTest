using Systems.SoundSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActivateBuildPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _buildPanel;
        private Button _button;

        private void Awake()
        {
            _buildPanel.SetActive(false);
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ShowBuildPanel);
        }

        private void ShowBuildPanel()
        {
            AudioSystem.Instance.PlaySfx(SfxType.ClickBuilding);
            _buildPanel.SetActive(true);
        }
    }
}