using UnityEngine;

public class EnableEndScreen : MonoBehaviour
{
    [SerializeField] private GameObject objectToEnable;

    [SerializeField] private string playerTag = "Player";

    private Dialogue endDialogue;

    private void Awake()
    {
        endDialogue = GetComponent<Dialogue>();
        if (endDialogue != null)
        {
            endDialogue.SetManualTriggerOnly(true);
            endDialogue.OnDialogueFinished += ShowEndScreen;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Biztonsági ellenõrzés: Csak akkor jöhet elõ a végképernyõ, ha a zongora meg van oldva!
            if (PlayerPrefs.GetInt("PianoPuzzle_Solved", 0) == 1)
            {
                // Ha hozzáadtad a Dialogue komponenst, ÉS be is húztad a Text-et (így nem crashel ki)
                if (endDialogue != null && endDialogue.textComponent != null)
                {
                    endDialogue.SetDialogueID("END_GAME");
                    endDialogue.TriggerDialogue(true);
                }
                else
                {
                    // Ha a szöveg nincs beállítva, egybõl beadja a végét, hogy ne ragadj be!
                    ShowEndScreen();
                }
            }
        }
    }

    private void ShowEndScreen()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
        else
        {
            Debug.LogError("Hiba: Nincs behúzva az 'Object To Enable' változóba az End Screen az Inspectorban!");
        }
    }

    private void OnDestroy()
    {
        if (endDialogue != null)
        {
            endDialogue.OnDialogueFinished -= ShowEndScreen;
        }
    }
}
