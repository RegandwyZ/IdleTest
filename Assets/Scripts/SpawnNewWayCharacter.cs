using System.Collections;
using UnityEngine;
using Character;
using Shop;



public class SpawnNewWayCharacter : MonoBehaviour
{
    [SerializeField] private CharacterData[] _newCharacterData;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Path _pathToMarket;
    [SerializeField] private Path _pathToTrain;
    [SerializeField] private ShopData[] _shopData;
    [SerializeField] private Transform _centerPoint;
    
    private int _count = 12;
    
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
            
            ShopData[] shuffledShops = ShuffleShops(_shopData);
            
            citizen.SetData(shuffledShops, _centerPoint.position);
            citizen.SetPathToMarketPlace(_pathToMarket);
            citizen.SetPathToTrainPlace(_pathToTrain);

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