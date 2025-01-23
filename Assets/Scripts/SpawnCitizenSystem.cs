using System.Collections;
using PathSystem;
using UnityEngine;
using Shop;
using CharacterController = Character.CharacterController;


public class SpawnCitizenSystem : MonoBehaviour
{
    [SerializeField] private CharacterController[] _newCharacterData;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private ShopData[] _startShopData;
    [SerializeField] private BuildSystem _buildSystem;
    
    [SerializeField] private CitizenPathHolder _pathHolder;
    
    private Transform _centerPoint;
    private int _count = 12;
    
    public void Initialize()
    {
        _buildSystem.OnShopPurchased += AddShopToArray;
        _centerPoint = _pathHolder.CentralPoint;
    }

    private void OnDestroy()
    {
        _buildSystem.OnShopPurchased -= AddShopToArray;
    }

    private void AddShopToArray(ShopData newShop)
    {
        var updatedShops = new ShopData[_startShopData.Length + 1];
        for (int i = 0; i < _startShopData.Length; i++)
        {
            updatedShops[i] = _startShopData[i];
        }
        updatedShops[^1] = newShop; 
        _startShopData = updatedShops; 
    }
    
    public void SpawnNewWay()
    {
        StartCoroutine(SpawnCitizen());
    }

    private IEnumerator SpawnCitizen()
    {
        for (int i = 0; i < _count; i++)
        {
            var randomIndex = Random.Range(0, _newCharacterData.Length);
            var citizen = Instantiate(_newCharacterData[randomIndex], _spawnPoints[i].position, _spawnPoints[i].rotation);
            
            ShopData[] shuffledShops = ShuffleShops(_startShopData);
            
            citizen.SetData(shuffledShops, _centerPoint.position);
            citizen.SetPathToMarketPlace(_pathHolder.CitizenPathToMarket);
            citizen.SetPathToTrainPlace(_pathHolder.CitizenPathToTrain);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private ShopData[] ShuffleShops(ShopData[] original)
    {
        ShopData[] shuffled = (ShopData[])original.Clone();
        
        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }

        return shuffled;
    }
}