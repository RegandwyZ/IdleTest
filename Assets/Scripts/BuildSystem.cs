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
        public ChangeButtonToImage ChangeButtonToImage;
    }

    [SerializeField] private List<ShopConfig> _shopConfigs;

    public void InitializeShopConfigs()
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
            if (config.ChangeButtonToImage != null)
            {
                config.ChangeButtonToImage.Change();
            }

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
                
                if (config.ChangeButtonToImage != null)
                {
                    config.ChangeButtonToImage.Change();
                }
                
                OnShopPurchased?.Invoke(config.ShopData);
                CurrentProgress.Instance.AddBuilding(config.Type);
            }
        }
    }
}
