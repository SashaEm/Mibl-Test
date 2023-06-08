using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CapsuleSurvivor.Game.UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _finalTime;
        [SerializeField] private Button _restartButton;
    
        private void Awake()
        {
            _restartButton.onClick.AddListener(OnGameRestartPressed);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnGameRestartPressed);
        }

        private void OnGameRestartPressed()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SetData(float finalTime)
        {
            _finalTime.text = $"{TimeSpan.FromSeconds(finalTime):mm\\:ss}";
        }
    }
}
