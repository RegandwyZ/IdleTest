using System.Collections;
using Citizen;
using SpawnSystem;
using UnityEngine;


namespace Train
{
    public class TrainMoving : MonoBehaviour
    {
        [SerializeField] private Transform _pointA;
        [SerializeField] private Transform _pointB;

        [SerializeField] private SpawnCitizenSystem _spawnCitizenSystem;

        private const float SPEED = 55f;

        private const float STATION_STOP_DURATION = 2f;
        private const float TRAIN_AWAY_DURATION = 2f;

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
                yield return new WaitForSeconds(STATION_STOP_DURATION);

                DeSpawnCharacters();

                yield return StartCoroutine(MoveToPoint(_pointA.position));
                yield return new WaitForSeconds(TRAIN_AWAY_DURATION);
            }
        }

        private IEnumerator MoveToPoint(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, SPEED * Time.deltaTime);
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