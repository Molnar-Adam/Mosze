using UnityEngine;

/// Ha egy objektumnak el kell tűnnie (megsemmisülnie) a pályán, miután egy másik pályán történt valami (pl. egy kar meghúzása), ez a script ellenőrzi a GameState-t betöltéskor.
public class CrossSceneDestroyer : MonoBehaviour
{
    /// Az objektum egyedi azonosítója (pontosan egyeznie kell a kapcsolóban megadott szöveggel).
    [SerializeField] private string objectIdentifier;

    private void Start()
    {
        // Ha az objektum azonosítója szerepel a megsemmisített elemek listájában, akkor azonnal eltüntetjük.
        if (!string.IsNullOrEmpty(objectIdentifier) && GameState.destroyedObjects.Contains(objectIdentifier))
        {
            Destroy(gameObject);
        }
    }
}