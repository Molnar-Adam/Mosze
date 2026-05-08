using UnityEngine;
using TMPro;

/// Felvehető tárgyakat kezelő osztály.
public class ItemPickup : MonoBehaviour
{
    /// A "Nyomd meg az E-t a felvételhez" UI szöveg.
    [SerializeField] private TextMeshProUGUI PickupText;
    
    /// A felvehető tárgy egyedi azonosítója a mentési rendszer felé.
    [SerializeField] private string itemId;
    
    /// Az életerőt vagy a felvételt követő opcionális respawn/időzítő pozíciót tárolja.
    [SerializeField] private Transform respawnLocation;

    [SerializeField] private GameObject doorToOpen;


    private bool pickupAllowed;
    private Timer timer;
    private Transform playerTransform;
    private Dialogue dialogueScript;

    /// Végrehajtódik közvetlenül az inicializáció kezdetén. Lekéri a Timer referenciát.
    private void Awake()
    {
        ResolveTimerReference();
        // Megkeressük a Dialogue szkriptet ugyanazon az objektumon
        dialogueScript = GetComponent<Dialogue>();
    }

    /// Elrejti a prompt szöveget, és eltávolítja a tárgyat, ha már korábban felvették.
    private void Start()
    {
        if (PickupText != null)
        {
            PickupText.gameObject.SetActive(false);
        }

        if (string.IsNullOrWhiteSpace(itemId))
        {
            return;
        }

        if (CollectedItemsState.IsCollected(itemId))
        {
            Destroy(gameObject);
        }
    }

    /// Képkockánként ellenőrzi a felvétel gomb (E) lenyomását, ha a játékos a zónában van.
    private void Update()
    {
        if(pickupAllowed && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    /// Aktiválja a felvétel lehetőségét és a feliratot, amikor a játékos belép a triggerbe.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;

            if (PickupText != null)
            {
                PickupText.gameObject.SetActive(true);
    /// Kikapcsolja a felvétel lehetőségét és eltünteti a feliratot, ha a játékos kilép.
            }

            pickupAllowed = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = null;

    /// Felveszi a tárgyat, triggereli a mentést, gyógyíthat, respawnolhat, vagy megállíthat egy időzítőt, majd megsemmisíti a GameObjectet.
            if (PickupText != null)
            {
                PickupText.gameObject.SetActive(false);
            }

            pickupAllowed = false;
        }
    }

    private void Pickup()
    {
        // Megnézzük, van-e Dialogue szkript ezen a tárgyon
        Dialogue dialogueScript = GetComponent<Dialogue>();

        if (dialogueScript != null)
        {
            // 1. Elindítjuk a szöveget
            dialogueScript.StartDialogue();

            // 2. ELTÜNTETJÜK a kötelet a szemünk elől (kép és ütközés kikapcsolása)
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().enabled = false;

            if (GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().enabled = false;

            // 3. A "Nyomd meg az E-t" szöveget is levesszük
            if (PickupText != null)
                PickupText.gameObject.SetActive(false);

            // Itt megállunk! Nem hívunk Destroy-t, mert a Dialogue.cs fogja 
            // meghívni a RealPickupLogic-ot, ha vége a szövegnek.
            return;
        }

        // Ha nincs rajta dialógus, akkor azonnal lefut a rendes logika
        RealPickupLogic();
    }

    /// Ez a függvény végzi el a tényleges felvételt, miután a szöveg lement (vagy ha nincs szöveg).
    public void RealPickupLogic()
    {
        ResolveTimerReference();

        bool isHealItem = itemId.StartsWith("Heal");

        // Időzített szoba és teleport kezelése
        if (!isHealItem && timer != null && timer.IsTimerRunning && playerTransform != null)
        {
            if (respawnLocation != null)
            {
                playerTransform.position = respawnLocation.position;
                Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }
            }

            timer.TriggerTimerEnd(playerTransform);
        }

        // Tárgy mentése a rendszerbe
        if (!CollectedItemsState.TryCollect(itemId))
        {
            return;
        }

        // Ajtónyitás, ha van hozzárendelve fal
        if (doorToOpen != null)
        {
            doorToOpen.SetActive(false);
        }

        // Gyógyítás
        if (isHealItem && playerTransform != null)
        {
            PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(-2);
            }
        }

        if (PickupText != null)
        {
            PickupText.gameObject.SetActive(false);
        }

        // Végleg töröljük az objektumot
        Destroy(gameObject);
    }

    private void ResolveTimerReference()
    {
        if (timer != null)
        {
            return;
        }

        timer = FindFirstObjectByType<Timer>();
    }
}
