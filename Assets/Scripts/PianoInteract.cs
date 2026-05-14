using TMPro;
using UnityEngine;

/// A Zongora interakciót és az ahhoz tartozó UI panelt vezérlő szkript.
public class PianoInteract : MonoBehaviour
{
    /// Globális változó, ami jelzi van-e éppen a képernyőn megnyitott zongora menü.
    public static bool IsAnyPianoUIOpen { get; private set; }
    
    /// Eltárolja annak a frame-nek a számát, amelyikben az utolsó zongora menü Escape-pel lett bezárva.
    public static int LastPianoClosedWithEscapeFrame { get; private set; } = -1;

    /// A zongora billentyűit tartalmazó UI Panel.
    [SerializeField] private GameObject pianoUI;
    
    /// A "Nyomd meg az E-t" szöveges prompt.
    [SerializeField] private TextMeshProUGUI interactText;
    
    /// A felvételi gomb az interakció elindításához.
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    
    /// A játékost azonosító tag string.
    [SerializeField] private string playerTag = "Player";
    
    /// A dialógus menedzser a zongora szükségleteinek bemutatásához.
    [SerializeField] private Dialogue requirementDialogue;

    private bool canInteract;
    private bool isOpen;
    private bool isInteractionLocked;

    /// Újratöltéskor automatikusan nullázza a zongora statikus állapotait a memóriában.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        IsAnyPianoUIOpen = false;
        LastPianoClosedWithEscapeFrame = -1;
    }

    /// Jelenet betöltésekor felkészíti a dialógus rendszert és alapértelmezetten elrejti a UI elemeket.
    private void Awake()
    {
        if (requirementDialogue == null)
        {
            requirementDialogue = GetComponent<Dialogue>();
        }

        if (requirementDialogue != null)
        {
            requirementDialogue.SetManualTriggerOnly(true);
            requirementDialogue.OnDialogueFinished += OnPianoDialogueFinished;
        }

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }

        if (pianoUI != null)
        {
            pianoUI.SetActive(false);
        }
    }

    private void OnPianoDialogueFinished()
    {
        if (CollectedItemsState.CollectedCount >= CollectedItemsState.RequiredItemCount)
        {
            if (pianoUI != null)
            {
                pianoUI.SetActive(true);
                isOpen = true;
                IsAnyPianoUIOpen = true;
                Time.timeScale = 0f;

                if (interactText != null)
                {
                    interactText.gameObject.SetActive(false);
                }
            }
        }
    }

    /// Képkockánként figyel az interakcióba lépésre vagy az Escape lenyomásával való kilépésre.
    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            LastPianoClosedWithEscapeFrame = Time.frameCount;
            ClosePiano();
            return;
        }

        if (isInteractionLocked || !canInteract)
        {
            return;
        }

        if (Input.GetKeyDown(interactKey))
        {
            OpenPiano();
    /// Ha a játékos belép a zongora zónájába, engedélyezi az interakciót nyitva az E prompttal.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
        {
            return;
        }

        if (isInteractionLocked)
        {
            return;
        }

        canInteract = true;

        if (interactText != null)
        {
            interactText.gameObject.SetActive(true);
    /// Ha a játékos kilép, letiltja az interakciót és eltünteti a feliratot.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
        {
            return;
        }

        canInteract = false;

    /// Megnyitja a zongora UI-t és lefagyasztja az időt, amennyiben minden szükséges tárgy össze lett gyűjtve.
    /// Ha nem, elindít egy hiányt közlő dialógust (requirementDialogue).
        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void OpenPiano()
    {
        if (isInteractionLocked)
        {
            return;
        }

        if (requirementDialogue != null)
        {
            if (CollectedItemsState.CollectedCount < CollectedItemsState.RequiredItemCount)
            {
                requirementDialogue.SetDialogueID("PIANO_MISSING_KEYS");
            }
            else
            {
                requirementDialogue.SetDialogueID("PIANO_HAS_KEYS");
            }
            requirementDialogue.TriggerDialogue();
        }
        else if (CollectedItemsState.CollectedCount >= CollectedItemsState.RequiredItemCount)
        {
            // Fallback for missing dialogue component
            if (pianoUI != null)
            {
                pianoUI.SetActive(true);
                isOpen = true;
                IsAnyPianoUIOpen = true;
                Time.timeScale = 0f;

                if (interactText != null)
                {
                    interactText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ClosePiano()
    {
        if (pianoUI != null)
        {
            pianoUI.SetActive(false);
        }

    /// Teljesen lezárja a zongora gombjait és használhatóságát.
        isOpen = false;
        IsAnyPianoUIOpen = false;
        Time.timeScale = 1f;

        if (canInteract && interactText != null)
        {
            interactText.gameObject.SetActive(true);
        }
    }

    public void LockInteraction()
    {
        isInteractionLocked = true;
    /// Alaphelyzetbe állítja a zongora állapotát.
        canInteract = false;

        if (isOpen)
        {
            ClosePiano();
        }

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void ResetInteractionState()
    {
        isInteractionLocked = false;
        canInteract = false;
        isOpen = false;
    /// Ha a szkript le lesz tiltva, minden UI elemet kikapcsol.
    
        if (pianoUI != null)
        {
            pianoUI.SetActive(false);
        }

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }

        IsAnyPianoUIOpen = false;
        LastPianoClosedWithEscapeFrame = -1;
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        if (requirementDialogue != null)
        {
            requirementDialogue.OnDialogueFinished -= OnPianoDialogueFinished;
        }
    }

    private void OnDisable()
    {
        canInteract = false;

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }

        if (isOpen)
        {
            ClosePiano();
        }
        else
        {
            IsAnyPianoUIOpen = false;
        }
    }
}
