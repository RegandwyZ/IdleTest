using UnityEngine;

namespace Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private GameObject _shopPanel;

        private void Awake()
        {
            _shopPanel.SetActive(false);
        }

        public void OpenShopPanel()
        {
            _shopPanel.SetActive(true);
        }

        public void CloseShopPanel()
        {
            _shopPanel.SetActive(false);
        }
        
        
    }
}