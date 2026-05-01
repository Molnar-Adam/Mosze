using UnityEngine;

/// Egy adott jeleneten belüli spawn pontot reprezentáló osztály, azonosítóval ellátva.
public class SceneSpawnPoint : MonoBehaviour
{
    /// A spawn pont egyedi azonosítója.
    [SerializeField] private string spawnId;

    /// Visszaadja a spawn pont string azonosítóját.
    public string SpawnId => spawnId;
}
