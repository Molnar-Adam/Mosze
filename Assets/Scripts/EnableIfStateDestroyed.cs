using UnityEngine;

/// AI generált kód

/// A GameObject aktív állapotát egy mentett játékállapot alapján állítja be.
/// Az objektum csak akkor lesz aktív, ha a megadott azonosító szerepel a megsemmisített 
/// objektumok listájában, vagy ennek fordítottja, ha az invertCheck be van kapcsolva.
public class EnableIfStateDestroyed : MonoBehaviour
{
    /// Az azonosító, amelyet a GameState.destroyedObjects gyűjteményben keresünk.
    [SerializeField] private string destroyStateIdentifier;

    /// Ha igaz, akkor a feltétel eredménye invertálódik:
    /// az objektum akkor lesz aktív, ha az azonosító NEM található meg.
    [SerializeField] private bool invertCheck = false;

    /// Az objektum inicializálásakor lefut.
    /// Meghatározza, hogy az objektum aktív legyen-e a mentett állapot alapján.
    private void Awake()
    {
        // Ha nincs megadva azonosító, nincs mit ellenőrizni.
        if (string.IsNullOrEmpty(destroyStateIdentifier))
        {
            return;
        }

        // Ellenőrzi, hogy az azonosító szerepel-e a megsemmisített objektumok listájában.
        bool containsKey = GameState.destroyedObjects.Contains(destroyStateIdentifier);

        // Alapértelmezés szerint az objektum akkor aktív, ha az azonosító megtalálható.
        bool shouldBeActive = containsKey;

        // Igény esetén megfordítja az eredményt.
        if (invertCheck)
        {
            shouldBeActive = !shouldBeActive;
        }

        // Beállítja az objektum aktív állapotát.
        gameObject.SetActive(shouldBeActive);
    }
}