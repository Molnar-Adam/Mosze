using UnityEngine;

public class EnableIfStateDestroyed : MonoBehaviour
{
    [SerializeField] private string destroyStateIdentifier;
    [SerializeField] private bool invertCheck = false;

    private void Awake()
    {
        if (string.IsNullOrEmpty(destroyStateIdentifier))
        {
            return;
        }

        bool containsKey = GameState.destroyedObjects.Contains(destroyStateIdentifier);

        bool shouldBeActive = containsKey;
        if (invertCheck) shouldBeActive = !shouldBeActive;

        gameObject.SetActive(shouldBeActive);
    }
}
