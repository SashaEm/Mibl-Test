using UnityEngine;

namespace CapsuleSurvivor.Game.Level
{
    public abstract class Spawnable : MonoBehaviour
    {
        [field:SerializeField] public int InitialSpawnCount { get; protected set; }
        [field:SerializeField] public float SpawnOvertimeInterval { get; protected set; }
    }
}
