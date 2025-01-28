using System.Collections;
using Citizen;
using Shop;
using Systems.PathSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.SpawnSystem
{
    public class SpawnCitizenSystem : MonoBehaviour
    {
        [SerializeField] private CitizenController[] _newCharacterData;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private ActiveShopHolder _activeShopHolder;
        [SerializeField] private CitizenPathHolder _pathHolder;
        
        private SpawnCounter _spawnCounter;
        private int _spawnCycleCounter;

        private void Awake()
        {
            _spawnCounter = new SpawnCounter(4);
            _spawnCycleCounter = 0;
        }

        public void SpawnNewWay()
        {
            StartCoroutine(SpawnCitizen());
        }

        private IEnumerator SpawnCitizen()
        {
            _spawnCycleCounter++;

            for (int i = 0; i < _spawnCounter.SpawnCount; i++)
            {
                var randomIndex = Random.Range(0, _newCharacterData.Length);
                var citizen = Instantiate(_newCharacterData[randomIndex], _spawnPoints[i % _spawnPoints.Length].position, _spawnPoints[i % _spawnPoints.Length].rotation);
                citizen.ConfigureCitizen();
                var shuffledShops = _activeShopHolder.GetShops();
            
                citizen.SetData(shuffledShops, _pathHolder.CentralPoint.position);
                citizen.SetPathTo(_pathHolder.CitizenPathToMarket, _pathHolder.CitizenPathToTrain);

                yield return new WaitForSeconds(0.1f);
            }

            if (_spawnCycleCounter % 5 == 0) 
            {
                _spawnCounter.IncreaseSpawnCount();
            }
        }
    }
}