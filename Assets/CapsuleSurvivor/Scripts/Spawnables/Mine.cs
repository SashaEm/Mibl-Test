using CapsuleSurvivor.Game.Level;
using CapsuleSurvivor.Game.Player;
using UnityEngine;

namespace CapsuleSurvivor.Game.Spawnables
{
    [RequireComponent(typeof(Collider))]
    public class Mine : Spawnable
    {
        [SerializeField] private GameObject _explosionParticles;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if(_explosionParticles)
                    _explosionParticles.SetActive(true);
                other.GetComponent<PlayerHealth>().Kill();
            }
        }
    }
}
