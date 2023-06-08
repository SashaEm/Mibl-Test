using System;
using UnityEngine;

namespace CapsuleSurvivor.Game.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public static event Action OnPlayerDied;
    
        public void Kill()
        {
            OnPlayerDied?.Invoke();
        }
    }
}
