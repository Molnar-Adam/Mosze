using UnityEngine;
using UnityEngine.SceneManagement;

/// A jelenetváltások utáni pontos spawnolást statikusan kezelő rendszer.
public static class SceneSpawnSystem
{
    /// A következő jelenetben keresendő spawn pont azonosítója.
    private static string pendingSpawnId;
    
    /// A teleportálandó játékos címkéje.
    private static string pendingPlayerTag = "Player";
    
    /// Jelzi, hogy az eseményfigyelés be lett-e már állítva.
    private static bool isInitialized;

    /// Eltárolja a következő jelenetbetöltéshez szükséges spawn adatokat.
    public static void SetNextSpawn(string spawnId, string playerTag)
    {
        pendingSpawnId = spawnId;

        if (!string.IsNullOrWhiteSpace(playerTag))
        {
            pendingPlayerTag = playerTag;
        }
    }

    /// Játék újraindításakor (Editorban) alapállapotba állítja a statikus változókat.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        pendingSpawnId = null;
        pendingPlayerTag = "Player";
    /// Feliratkozik a jelenet betöltése eseményre közvetlenül a játék kezdete előtt.
        isInitialized = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    /// Új jelenet betöltésekor megkeresi a beállított spawn pontot és oda transzportálja a játékost.
        isInitialized = true;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrWhiteSpace(pendingSpawnId))
        {
            return;
        }

        SceneSpawnPoint[] spawnPoints = Object.FindObjectsByType<SceneSpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SceneSpawnPoint point = spawnPoints[i];
            if (point == null || point.SpawnId != pendingSpawnId)
            {
                continue;
            }

            GameObject player = GameObject.FindGameObjectWithTag(pendingPlayerTag);
            if (player == null)
            {
                Debug.LogWarning($"No GameObject found with tag '{pendingPlayerTag}' after loading scene '{scene.name}'.");
                pendingSpawnId = null;
                return;
            }

            player.transform.position = point.transform.position;
            pendingSpawnId = null;
            return;
        }

        Debug.LogWarning($"Spawn point '{pendingSpawnId}' was not found in scene '{scene.name}'.");
        pendingSpawnId = null;
    }
}
