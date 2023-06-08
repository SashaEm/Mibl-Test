using System.Collections.Generic;
using UnityEngine;

namespace CapsuleSurvivor.Game.Enemy
{
    public class EnemiesManager : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private HashSet<Spawnables.Enemy> _enemies = new();
        private bool _isGameRunning;

        private void Awake()
        {
            Spawnables.Enemy.OnNewEnemySpawned += RegisterEnemy;
            GameStatusTracker.GameStarted += OnGameStarted;
            GameStatusTracker.GameEnded += OnGameEnded;
        }
    
        private void OnDestroy()
        {
            Spawnables.Enemy.OnNewEnemySpawned -= RegisterEnemy;
            GameStatusTracker.GameStarted -= OnGameStarted;
            GameStatusTracker.GameEnded -= OnGameEnded;
        }
    
        private void OnGameStarted()
        {
            _isGameRunning = true;
        }

        private void OnGameEnded()
        {
            _isGameRunning = false;
        }
    
        private void RegisterEnemy(Spawnables.Enemy newEnemy)
        {
            _enemies.Add(newEnemy);
        }

        private void Update()
        {
            UpdateEnemiesMovementTarget(_player.position);
        }

        private void UpdateEnemiesMovementTarget(Vector3 targetPosition)
        {
            if (!_isGameRunning)
                return;
            foreach (Spawnables.Enemy enemy in _enemies)
            {
                if (!enemy)
                {
                    _enemies.Remove(enemy);
                    continue;
                }
                enemy.UpdateTargetPosition(targetPosition);
            }
        }
    }
}
