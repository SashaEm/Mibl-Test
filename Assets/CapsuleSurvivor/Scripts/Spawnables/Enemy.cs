using System;
using CapsuleSurvivor.Game.Level;
using CapsuleSurvivor.Game.Player;
using UnityEngine;
using UnityEngine.AI;

namespace CapsuleSurvivor.Game.Spawnables
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Spawnable
    {
        [Range(0.1f, 100f), SerializeField] private float _movementSpeed = 9f;
        [Range(0.1f, 40f), SerializeField] private float _movementAcceleration = 25f;

        public static event Action<Enemy> OnNewEnemySpawned;

        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _movementSpeed;
            _navMeshAgent.acceleration = _movementAcceleration;
        
            OnNewEnemySpawned?.Invoke(this);
        }

        public void UpdateTargetPosition(Vector3 target)
        {
            _navMeshAgent.destination = target;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerHealth>().Kill();
            }
        }
    }
}
