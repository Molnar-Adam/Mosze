using UnityEngine;
using TMPro;

/// A villanykapcsolót kezelő szkript, amellyel felkapcsolható az áram.
public class LightSwitch : MonoBehaviour
{
    /// A UI szöveg, ami jelzi, hogy interakcióba lehet lépni a kapcsolóval.
    [SerializeField] private TextMeshProUGUI InteractText;
    
    /// Jelzi, hogy a játékos a kapcsoló közelében van-e.
    private bool InteractAllowed = false;
    
    /// A játékos pozíciója.
    private Transform playerTransform;

    /// Annak az objektumnak a globális azonosítója (pl. egy másik pályán), amit el akarunk pusztítani a kapcsoló meghúzásakor.
    [SerializeField] private string targetObjectToDestroy;

    /// A teleportálás célállomása (hova kerüljön a játékos a kar meghúzása után).
    [SerializeField] private Transform teleportDestination;

    /// A kapcsoló grafikus megjelenítője.
    [SerializeField] private SpriteRenderer spriteRenderer;

    /// Kikapcsolt állapot (alap) képe.
    [SerializeField] private Sprite offSprite;

    /// Bekapcsolt állapot képe.
    [SerializeField] private Sprite onSprite;

    /// Kezdetben elrejti az interakciós feliratot.
    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        UpdateSprite();

        if (InteractText != null)
        {
            InteractText.gameObject.SetActive(false);
        }


    }

    /// Várja az "E" gomb lenyomását a felkapcsoláshoz, ha a zónán belül vagyunk.
    private void Update()
    {
        if(InteractAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PullLever();
        }
    }

    /// Ha a játékos belép a kapcsoló zónájába, engedélyezi a felkapcsolást és megjeleníti a feliratot.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;

            if (InteractText != null)
            {
                InteractText.gameObject.SetActive(true);
    /// Ha a játékos elhagyja a kapcsoló zónáját, eltünteti a feliratot és tiltja a műveletet.
            }

            InteractAllowed = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = null;

    /// Felkapcsolja az áramot (GameState.powerOn = true).
            if (InteractText != null)
            {
                InteractText.gameObject.SetActive(false);
            }

            InteractAllowed = false;
        }
    }

    public void PullLever()
    {
        // Igény szerint kapcsolóként (ki-be) is működhetne: GameState.powerOn = !GameState.powerOn;
        GameState.powerOn = true;
        Debug.Log("Power ON");

        UpdateSprite();

        // Ha meg van adva egy célállomás, oda teleportáljuk a játékost.
        if (teleportDestination != null && playerTransform != null)
        {
            playerTransform.position = teleportDestination.position;
        }

        // Regisztráljuk, hogy ezt az objektumot "megsemmisítettük", így ha átmegyünk a másik pályára, eltűnhet.
        if (!string.IsNullOrEmpty(targetObjectToDestroy))
        {
            GameState.destroyedObjects.Add(targetObjectToDestroy);
        }
    }

    /// Frissíti a sprite-ot a GameState.powerOn globális állapot alapján.
    private void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GameState.powerOn ? onSprite : offSprite;
        }
    }

}

