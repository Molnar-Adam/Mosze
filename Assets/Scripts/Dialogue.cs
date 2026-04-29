using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [SerializeField] private string dialogueID;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private bool requireInteractKey = true;
    [SerializeField] private bool manualTriggerOnly = false;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject interactUI;
    public string[] lines;
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
        if (!dialogueStarted)
        {
            if (!manualTriggerOnly && isPlayerInRange && requireInteractKey && !hasTriggered)
            {
                if (Input.GetKeyDown(interactKey))
                {
                    if (interactUI != null)
                    {
                        interactUI.SetActive(false);
                    }
                    TriggerDialogue(false);
                }
            }
            return;
        }

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

    void StartDialogue()
    {
        dialogueStarted = true;
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
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
        }
    }
}
