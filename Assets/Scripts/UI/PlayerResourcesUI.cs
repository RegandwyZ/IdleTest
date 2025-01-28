using Systems.ResourcesSystem;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerResourcesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMoney;

        private void Start()
        {
            ResourcesSystem.Instance.OnMoneyChanged += UpdateMoneyText;
            UpdateMoneyText(ResourcesSystem.Instance.Money);
        }

        private void OnDisable()
        {
            ResourcesSystem.Instance.OnMoneyChanged -= UpdateMoneyText;
        }

        private void UpdateMoneyText(int money)
        {
            _textMoney.text = $"${money}";
        }
    }
}