using UnityEngine;

public class DialogueTriggerHelper : MonoBehaviour
{
    public Dialogue dialogueScript; // Ide húzd be a Dialogue szkriptet
    public GameObject interactPrompt; // Opcionális: "Press E" felirat
    private bool isPlayerNearby;

    void Start()
    {
        if (dialogueScript == null) dialogueScript = GetComponent<Dialogue>();
        if (interactPrompt != null) interactPrompt.SetActive(false);

        // Fontos: Kapcsoljuk ki az automatikus indulást!
        dialogueScript.SetAutoTrigger(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            dialogueScript.TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }
}