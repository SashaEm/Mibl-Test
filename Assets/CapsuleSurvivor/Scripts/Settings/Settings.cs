using UnityEngine;

namespace CapsuleSurvivor.Settings
{
    public class Settings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }
    }
}
