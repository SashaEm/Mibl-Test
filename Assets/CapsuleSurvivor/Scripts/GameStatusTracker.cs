using System;
using System.Collections;
using System.Threading.Tasks;
using CapsuleSurvivor.Game.Player;
using CapsuleSurvivor.Game.UI;
using TMPro;
using UnityEngine;

namespace CapsuleSurvivor.Game
{
    public class GameStatusTracker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _gameTimeDisplay;
        [SerializeField] private EndScreen _endScreen;

        private float _timeElapsed;
        private bool _isGameActive;

        public static event Action GameStarted;
        public static event Action GameEnded;

        private void Awake()
        {
            PlayerHealth.OnPlayerDied += OnGameEnded;
        }

        private void Start()
        {
            StartGame();
        }

        private void OnDestroy()
        {
            PlayerHealth.OnPlayerDied -= OnGameEnded;
        }

        private async void OnGameEnded()
        {
            _isGameActive = false;
            GameEnded?.Invoke();

            await Task.Delay(500);
            _endScreen.gameObject.SetActive(true);
            _endScreen.SetData(_timeElapsed);
        }

        private void StartGame()
        {
            GameStarted?.Invoke();
            _isGameActive = true;
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            float startTime = Time.time;
            _timeElapsed = 0f;

            while (_isGameActive)
            {
                _timeElapsed = Time.time - startTime;

                _gameTimeDisplay.text = $"{TimeSpan.FromSeconds(_timeElapsed):mm\\:ss}";

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
