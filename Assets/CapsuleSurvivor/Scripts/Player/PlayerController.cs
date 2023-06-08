using System;
using UnityEngine;
using UnityEngine.AI;

namespace CapsuleSurvivor.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Range(0.1f, 100f), SerializeField] private float _movementSpeed = 10f;
        [Range(0.1f, 40f), SerializeField] private float _movementAcceleration = 10f;
    
        [SerializeField] private NavMeshAgent _playerAgent;

        private Camera _camera;
        private bool _isStopInvoked;
    
        /// <summary>
        /// Provides player target position Vector3, bool isMovementStarted
        /// </summary>
        public event Action<Vector3, bool> OnPlayerMovementStateChanged;

        private void Awake()
        {
            _camera = Camera.main;
            _playerAgent.speed = _movementSpeed;
            _playerAgent.acceleration = _movementAcceleration;

            GameStatusTracker.GameStarted += OnGameStarted;
            GameStatusTracker.GameEnded += OnGameEnded;
        }

        private void OnDestroy()
        {
            GameStatusTracker.GameStarted -= OnGameStarted;
            GameStatusTracker.GameEnded -= OnGameEnded;
        }

        private void OnGameStarted()
        {
            _playerAgent.enabled = true;
        }

        private void OnGameEnded()
        {
            _playerAgent.enabled = false;
        }

        private void Update()
        {
            UpdateIsPlayerMoving();
        
            if (Input.GetMouseButton(0))
            {
                SetAgentTarget();
            }
        }

        private void UpdateIsPlayerMoving()
        {
            if (!_playerAgent.enabled)
                return;
            if (_playerAgent.remainingDistance < 0.1f && !_isStopInvoked)
            {
                OnPlayerMovementStateChanged?.Invoke(Vector3.zero, false);
                _isStopInvoked = true;
            }
        }

        private void SetAgentTarget()
        {
            if (!_playerAgent.enabled)
                return;
            Ray worldPosition = 
                _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));

            if (!Physics.Raycast(worldPosition, out RaycastHit rayHitInfo))
                return;

            if (!NavMesh.SamplePosition(rayHitInfo.point, out NavMeshHit hit, Single.PositiveInfinity, NavMesh.AllAreas))
                return;

            _playerAgent.destination = hit.position;
            _isStopInvoked = false;
            OnPlayerMovementStateChanged?.Invoke(hit.position, true);
        }
    }
}
