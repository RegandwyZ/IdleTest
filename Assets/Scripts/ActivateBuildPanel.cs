using UnityEngine;
using UnityEngine.UI;

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
        _buildPanel.SetActive(true);
    }
    
}
