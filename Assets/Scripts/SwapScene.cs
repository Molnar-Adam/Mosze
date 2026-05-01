using UnityEngine;
using UnityEngine.SceneManagement;

/// Egy kijelölt zónába (Trigger) lépéskor betölti a megadott jelenetet és átadja a következő spawn pontot.
public class SwapScene : MonoBehaviour
{
    /// A betöltendő új jelenet neve.
    [SerializeField] private string targetSceneName;
    
    /// A céljelenetben lévő spawn pont azonosítója.
    [SerializeField] private string targetSpawnId;

    /// A játékos objektum címkéje, amely aktiválhatja az átmenetet.
    [SerializeField] private string playerTag = "Player";
    
    /// Megabja, hogy a trigger csak egyetlen egyszer aktiválódhat-e.
    [SerializeField] private bool triggerOnlyOnce = true;

    ///Nyilvántartja, hogy a jelenetváltás már elindult-e.
    private bool wasTriggered;

    /// Játékos érzékelésekor regisztrálja az új spawn pontot és elindítja a jelenet betöltését.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOnlyOnce && wasTriggered)
		{
			return;
		}

		if (!collision.CompareTag(playerTag))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(targetSceneName))
		{
			Debug.LogWarning($"{name} has no targetSceneName set.", this);
			return;
		}

		wasTriggered = true;
		SceneSpawnSystem.SetNextSpawn(targetSpawnId, playerTag);
		SceneManager.LoadScene(targetSceneName);
	}
}
