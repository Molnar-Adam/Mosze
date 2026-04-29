using TMPro;
using UnityEngine;

public class PianoInteract : MonoBehaviour
{
    public static bool IsAnyPianoUIOpen { get; private set; }
    public static int LastPianoClosedWithEscapeFrame { get; private set; } = -1;

    [SerializeField] private GameObject pianoUI;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Dialogue requirementDialogue;

    private bool canInteract;
    private bool isOpen;
    private bool isInteractionLocked;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        IsAnyPianoUIOpen = false;
        LastPianoClosedWithEscapeFrame = -1;
    }

    private void Awake()
    {
        if (requirementDialogue == null)
        {
            requirementDialogue = GetComponent<Dialogue>();
        }

        if (requirementDialogue != null)
        {
            requirementDialogue.SetManualTriggerOnly(true);
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
        {
            return;
        }

        canInteract = false;

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

        if (CollectedItemsState.CollectedCount < CollectedItemsState.RequiredItemCount)
        {
            if (requirementDialogue != null)
            {
                requirementDialogue.TriggerDialogue();
            }

            return;
        }

        if (pianoUI == null)
        {
            return;
        }

        pianoUI.SetActive(true);
        isOpen = true;
        IsAnyPianoUIOpen = true;
        Time.timeScale = 0f;

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void ClosePiano()
    {
        if (pianoUI != null)
        {
            pianoUI.SetActive(false);
        }

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
