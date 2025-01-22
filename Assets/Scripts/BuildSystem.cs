using System;
using System.Collections.Generic;
using PlayerCurrentProgress;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSystem : MonoBehaviour
{
    public event Action<ShopData> OnShopPurchased;
    
    [Serializable]
    private struct ShopConfig
    {
        public ShopType Type;
        public int Price;
        public TextMeshProUGUI PriceText;
        public Button Button;
        public ShopData ShopData;
    }

    [SerializeField] private List<ShopConfig> _shopConfigs;

    private void Awake()
    {
        foreach (var config in _shopConfigs)
        {
            InitializeShop(config);
            config.ShopData.gameObject.SetActive(false); 
        }
        
        ActivateExistingShops();
    }

    private void InitializeShop(ShopConfig config)
    {
        config.PriceText.text = config.Price.ToString();
        config.Button.onClick.AddListener(() => BuyShop(config));
    }

    private void BuyShop(ShopConfig config)
    {
        if (ResourcesSystem.Instance.SpendMoney(config.Price))
        {
            var button = config.Button.GetComponentInParent<ChangeButtonToImage>();
            button?.Change();

            config.ShopData.gameObject.SetActive(true);
            OnShopPurchased?.Invoke(config.ShopData);
            CurrentProgress.Instance.AddBuilding(config.Type);
        }
    }
    
    private void ActivateExistingShops()
    {
        var existingBuildings = CurrentProgress.Instance.CurrentGameData.Buildings;

        foreach (var config in _shopConfigs)
        {
            if (existingBuildings.Exists(building => building.BuildingId == config.Type))
            {
                config.ShopData.gameObject.SetActive(true);
                
                var button = config.Button.GetComponentInParent<ChangeButtonToImage>();
                if (button != null)
                {
                    button.Change();
                }
                
                OnShopPurchased?.Invoke(config.ShopData);
            }
        }
    }
}
