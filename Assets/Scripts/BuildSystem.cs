using System;
using Shop;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    
    public event Action<ShopData> OnShopPurchased;
    
    [SerializeField] private ShopData _gardeningShop;
    [SerializeField] private ShopData _fashionShop;
    [SerializeField] private ShopData _pawnShop;
    [SerializeField] private ShopData _magicShop;
    
    private void Awake()
    {
        _gardeningShop.gameObject.SetActive(false);
        _fashionShop.gameObject.SetActive(false);
        _pawnShop.gameObject.SetActive(false);
        _magicShop.gameObject.SetActive(false);
    }
    
    public void BuyGardeningShop()
    {
        _gardeningShop.gameObject.SetActive(true);
        OnShopPurchased?.Invoke(_gardeningShop); 
    }
    
    public void BuyFashionShop()
    {
        _fashionShop.gameObject.SetActive(true);
        OnShopPurchased?.Invoke(_fashionShop);
    }
    
    public void BuyPawnShop()
    {
        _pawnShop.gameObject.SetActive(true);
        OnShopPurchased?.Invoke(_pawnShop);
    }
    
    public void BuyMagicShop()
    {
        _magicShop.gameObject.SetActive(true);
        OnShopPurchased?.Invoke(_magicShop);
    }
}
