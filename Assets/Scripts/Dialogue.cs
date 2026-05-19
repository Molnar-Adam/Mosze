using UnityEngine;
using TMPro;
using System.Collections;

    using System;

/// A játékban megjelenő dialógusokat és szövegdobozokat kezelő osztály.
/// Figyeli, hogy a játékos közel van-e, és kiírja a sorokat egymás után a képernyőre.
public class Dialogue : MonoBehaviour
{
    /// A TextMeshPro komponens, amelyen a szöveg fizikailag megjelenik az UI-on.
    public TextMeshProUGUI textComponent;

    /// A párbeszéd egyedi azonosítója (amely az EventManager JSON fájljában is szerepel).
    [SerializeField] private string dialogueID;

    /// Maga a szövegdoboz és a háttere, amit be- és ki lehet kapcsolni.
    [SerializeField] private GameObject dialogueBox;

    [Header("Map Specific Background")]
    /// A dialógusdoboz hátterét megjelenítő Image komponens.
    [SerializeField] private UnityEngine.UI.Image dialogueBackgroundImage;

    /// A pálya-specifikus háttérkép.
    [SerializeField] private Sprite mapSpecificBackground;

    public void SetDialogueID(string id)
    {
        dialogueID = id;
    }

    /// A Trigger terület, amibe belépve elindítható a szöveg.
    [SerializeField] private Collider2D triggerCollider;

    /// Ha igaz, a játékosnak le kell nyomnia egy gombot a beszéd elindításához. 
    /// Ha hamis, amint besétál, automatikusan elkezdődik.
    [SerializeField] private bool requireInteractKey = true;

    /// Ha igaz, maga a collider trigger esemény nem is indíthatja el, csak egy külső hívás.
    [SerializeField] private bool manualTriggerOnly = false;

    /// A billentyű, amit meg kell nyomni a beszélgetés kezdéséhez.
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    /// Az a kis felugró grafika, ami a beszélgetés előtt jelenik meg.
    [SerializeField] private GameObject interactUI;

    /// Esemény, ami kiváltódik a dialógus végén.
    public Action OnDialogueFinished;

    /// A párbeszéd sorai, amelyeket egymás után ír ki. Ha az EventManagerben van beállítva, felülírásra kerül.
    public string[] lines;

    /// A szöveg kiírásának sebessége (karakter per másodperc).
    public float textSpeed;

    private int index;
    private bool dialogueStarted;
    private bool hasTriggered;
    private bool isPlayerInRange;

    private void Awake()
    {
        if (dialogueBox == null && textComponent != null)
        {
            dialogueBox = textComponent.gameObject;
        }

        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider2D>();
        }
    }

    private void Start()
    {
        if (EventManager.Instance != null && !string.IsNullOrEmpty(dialogueID))
        {
            string[] loadedLines = EventManager.Instance.GetDialogueLines(dialogueID);
            if (loadedLines != null && loadedLines.Length > 0)
            {
                lines = loadedLines;
            }
        }

        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }

        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Ha nem megy a dialógus, csak az indítást figyeljük
        if (!dialogueStarted)
        {
            if (!manualTriggerOnly && isPlayerInRange && requireInteractKey && !hasTriggered)
            {
                if (Input.GetKeyDown(interactKey))
                {
                    if (interactUI != null) interactUI.SetActive(false);
                    TriggerDialogue(false);
                }
            }
            return; // Ha nincs dialógus, itt megáll a kód
        }

        // HA PEDIG MEGY A DIALÓGUS (dialogueStarted == true), akkor jön a léptetés:
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered || !collision.CompareTag("Player"))
        {
            return;
        }

        isPlayerInRange = true;
        
        if (manualTriggerOnly)
        {
            return;
        }

        if (requireInteractKey)
        {
            if (interactUI != null)
            {
                interactUI.SetActive(true);
            }
        }
        else
        {
            TriggerDialogue(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        isPlayerInRange = false;

        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }

    /// Nyilvános metódus a dialógus explicit elindítására és a szövegdoboz bekapcsolására.
    public void TriggerDialogue(bool destroyTriggerCollider = false)
    {
        if (hasTriggered)
        {
            return;
        }

        hasTriggered = true;

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
        }

        StartDialogue();

        if (destroyTriggerCollider && triggerCollider != null)
        {
            Destroy(triggerCollider);
        }
    }

    public void SetManualTriggerOnly(bool manual)
    {
        manualTriggerOnly = manual;
    }

    public void SetAutoTrigger(bool enabled)
    {
        requireInteractKey = !enabled;
    }

    public void StartDialogue()
    {
        // Megállítunk minden futó gépelést, hogy ne legyen duplázódás
        StopAllCoroutines();
        textComponent.text = string.Empty;

        lines = EventManager.Instance.GetDialogueLines(dialogueID);

        if (lines != null && lines.Length > 0)
        {
            if (dialogueBackgroundImage != null && mapSpecificBackground != null)
            {
                dialogueBackgroundImage.sprite = mapSpecificBackground;
            }

            dialogueBox.SetActive(true);
            index = 0;

            // EZT A SORT ADDD HOZZÁ (vagy ellenőrizd):
            dialogueStarted = true;

            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueStarted = false;
            hasTriggered = false;

            if (dialogueBox != null)
            {
                dialogueBox.SetActive(false);
            }

            ItemPickup item = GetComponent<ItemPickup>();
            if (item != null)
            {
                item.RealPickupLogic();
            }

            OnDialogueFinished?.Invoke();
        }
    }
}
