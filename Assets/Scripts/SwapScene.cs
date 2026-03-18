using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
	[SerializeField] private string targetSceneName;
	[SerializeField] private string targetSpawnId;

	[SerializeField] private string playerTag = "Player";
	[SerializeField] private bool triggerOnlyOnce = true;

	private bool wasTriggered;

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
