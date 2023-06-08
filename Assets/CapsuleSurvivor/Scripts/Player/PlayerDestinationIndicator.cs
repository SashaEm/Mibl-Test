using System;
using UnityEngine;

namespace CapsuleSurvivor.Game.Player
{
    public class PlayerDestinationIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameObject _indicator;

        private void Awake()
        {
            if (!_indicator)
            {
                gameObject.SetActive(false);
                throw new NullReferenceException("Indicator not assigned!");
            }
            _playerController.OnPlayerMovementStateChanged += OnPlayerMovementUpdated;
        }

        private void OnDestroy()
        {
            _playerController.OnPlayerMovementStateChanged -= OnPlayerMovementUpdated;
        }

        private void OnPlayerMovementUpdated(Vector3 arg1, bool arg2)
        {
            if (arg2)
            {
                if (!_indicator.activeInHierarchy)
                {
                    _indicator.SetActive(true);
                }

                _indicator.transform.position = arg1;
            }
            else
            {
                _indicator.SetActive(false);
            }
        }
    }
}
