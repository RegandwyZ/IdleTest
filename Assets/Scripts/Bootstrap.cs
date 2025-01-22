using System.Collections;
using UnityEngine;
using Shop;
using UnityEngine.Serialization;
using CharacterController = Character.CharacterController;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private CharacterController[] _newCharacterData;
    [SerializeField] private Transform[] _spawnPoints;
    [FormerlySerializedAs("_pathToMarket")] [SerializeField] private CitizenPath _citizenPathToMarket;
    [FormerlySerializedAs("_pathToTrain")] [SerializeField] private CitizenPath _citizenPathToTrain;
    [SerializeField] private ShopData[] _startShopData;
    [SerializeField] private Transform _centerPoint;
    [SerializeField] private BuildSystem _buildSystem;
    
    private int _count = 12;
    
    private void Awake()
    {
        _buildSystem.OnShopPurchased += AddShopToArray;
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
            citizen.SetPathToMarketPlace(_citizenPathToMarket);
            citizen.SetPathToTrainPlace(_citizenPathToTrain);

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