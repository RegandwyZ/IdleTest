using System.Collections;
using UnityEngine;
using Character;
using Shop;


public class SpawnNewWayCharacter : MonoBehaviour
{
    [SerializeField] private CharacterData _newCharacterData;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Path _path;
    [SerializeField] private ShopData[] _shopData;
    
    private int _count = 1;
    
    public void SpawnNewWay()
    {
        StartCoroutine(SpawnCitizen());
    }

    private IEnumerator SpawnCitizen()
    {
        for (int i = 0; i < _count; i++)
        {
            var citizen = Instantiate(_newCharacterData, _spawnPoints[i].position, _spawnPoints[i].rotation);
            ShopData[] shuffledShops = ShuffleShops(_shopData);
            
            citizen.SetData(shuffledShops);
            citizen.SetPathToMarketPlace(_path);

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