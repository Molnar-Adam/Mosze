using UnityEngine;

/// Aktiválja a játék végi screent, amikor a játékos belép a trigger területre.
public class EnableEndScreen : MonoBehaviour
{
    /// Az objektum, amelyet aktiválni kell a trigger aktiválásakor.
    [SerializeField] private GameObject objectToEnable;


    /// A játékost azonosító tag neve.
    [SerializeField] private string playerTag = "Player";

    /// Meghívódik, amikor egy 2D collider belép a trigger collider területére.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ellenőrzi, hogy a triggerbe belépő objektum rendelkezik-e a megadott taggel.
        if (other.CompareTag(playerTag))
        {
            // Csak akkor próbálja aktiválni az objektumot, ha az referencia szerint létezik.
            if (objectToEnable != null)
            {
                // Az objektum aktiválása.
                objectToEnable.SetActive(true);
            }
        }
    }
}