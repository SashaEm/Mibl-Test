using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CapsuleSurvivor.Game.Level
{
    [DefaultExecutionOrder(1100)]
    public class SpawnHandler : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _safeAreaAroundPlayerRadius = 5f;
        [SerializeField] private List<Spawnable> _spawnableObjects;
        [Space, SerializeField] private Transform _spawnedObjectsParent;

        private HashSet<Coroutine> _runningCoroutines = new();

        private void Awake()
        {
            SpawnInitialObjects();
            SetTimers();
            GameStatusTracker.GameEnded += OnGameEnded;
        }

        private void OnDestroy()
        {
            GameStatusTracker.GameEnded -= OnGameEnded;
        }

        private void OnGameEnded()
        {
            foreach (Coroutine coroutine in _runningCoroutines)
            {
                StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Starts countdowns for spawning objects that should be spawned at fixed rate.
        /// </summary>
        private void SetTimers()
        {
            foreach (Spawnable spawnableObject in _spawnableObjects)
            {
                if (spawnableObject.SpawnOvertimeInterval > 0)
                {
                    Coroutine runningCoroutine = StartCoroutine(SpawnObjectTimer(spawnableObject));
                    _runningCoroutines.Add(runningCoroutine);
                }
            }
        }

        private void SpawnInitialObjects()
        {
            foreach (Spawnable spawnableObject in _spawnableObjects)
            {
                for (int i = 0; i < spawnableObject.InitialSpawnCount; i++)
                {
                    SpawnObject(spawnableObject);
                }
            }
        }

        private IEnumerator SpawnObjectTimer(Spawnable spawnableObject)
        {
            float spawnTimerStart = Time.time;
            float spawnInterval = spawnableObject.SpawnOvertimeInterval;
            while (true)
            {
                if (Time.time - spawnTimerStart > spawnInterval)
                {
                    SpawnObject(spawnableObject);
                    spawnTimerStart = Time.time;
                }

                yield return null;
            }
        }
    
        private void SpawnObject(Spawnable spawnableObject)
        {
            Instantiate(spawnableObject, GetRandomPointOnNavMesh(), Quaternion.identity)
                .transform.SetParent(_spawnedObjectsParent);
        }

        private Vector3 GetRandomPointOnNavMesh()
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            int randomIndex = Random.Range(0, navMeshData.indices.Length - 3);
            Vector3 randomPoint = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[randomIndex]], navMeshData.vertices[navMeshData.indices[randomIndex + 1]], Random.value);
            randomPoint = Vector3.Lerp(randomPoint, navMeshData.vertices[navMeshData.indices[randomIndex + 2]], Random.value);

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(randomPoint, out navMeshHit, 1.0f, NavMesh.AllAreas))
            {
                // Checks if the spawn point is too close to the player.
                if (Vector3.Distance(navMeshHit.position, _player.position) < _safeAreaAroundPlayerRadius)
                    return GetRandomPointOnNavMesh();
                return navMeshHit.position;
            }

            // If a valid position is not found, try again recursively
            return GetRandomPointOnNavMesh();
        }
    }
}
