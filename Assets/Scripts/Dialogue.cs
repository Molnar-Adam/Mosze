using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private bool autoTriggerOnPlayerEnter = true;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool dialogueStarted;
    private bool hasTriggered;

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
        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }

    private void Update()
    {
        if (!dialogueStarted)
        {
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
        if (!autoTriggerOnPlayerEnter || hasTriggered || !collision.CompareTag("Player"))
        {
            return;
        }

        TriggerDialogue(true);
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

    public void SetAutoTrigger(bool enabled)
    {
        autoTriggerOnPlayerEnter = enabled;
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
