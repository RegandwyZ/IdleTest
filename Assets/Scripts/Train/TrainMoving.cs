using System.Collections;
using Citizen;
using UnityEngine;
using UnityEngine.Serialization;

namespace Train
{
    public class TrainMoving : MonoBehaviour
    {
        [SerializeField] private Transform _pointA; 
        [SerializeField] private Transform _pointB; 
        [FormerlySerializedAs("_bootstrap")] [FormerlySerializedAs("_spawnSystem")] [FormerlySerializedAs("_spawnNewWayCharacter")] [SerializeField] private SpawnCitizenSystem _spawnCitizenSystem;
        
        private readonly float _speed = 25f; 

        private void Start()
        {
            StartCoroutine(MoveTrain());
        }

        private IEnumerator MoveTrain()
        {
            while (true)
            {
                yield return StartCoroutine(MoveToPoint(_pointB.position));
                _spawnCitizenSystem.SpawnNewWay();
                yield return new WaitForSeconds(7f);
                
                DeSpawnCharacters();
                
                yield return StartCoroutine(MoveToPoint(_pointA.position));
                yield return new WaitForSeconds(5f);
            }
        }

        private IEnumerator MoveToPoint(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
                yield return null; 
            }
        
            transform.position = target;
        }

        private void DeSpawnCharacters()
        {
            CitizenController[] characters = FindObjectsByType<CitizenController>(sortMode: FindObjectsSortMode.None);
            foreach (var character in characters)
            {
                if (character.IsReadyToLeave) 
                {
                    Destroy(character.gameObject);
                }
            }
        }
    }
}
