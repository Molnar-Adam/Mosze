using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSpawnSystem
{
    private static string pendingSpawnId;
    private static string pendingPlayerTag = "Player";
    private static bool isInitialized;

    public static void SetNextSpawn(string spawnId, string playerTag)
    {
        pendingSpawnId = spawnId;

        if (!string.IsNullOrWhiteSpace(playerTag))
        {
            pendingPlayerTag = playerTag;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        pendingSpawnId = null;
        pendingPlayerTag = "Player";
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
