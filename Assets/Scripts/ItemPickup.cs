using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PickupText;
    [SerializeField] private string itemId;
    [SerializeField] private Transform respawnLocation;

    private bool pickupAllowed;
    private Timer timer;
    private Transform playerTransform;

    private void Awake()
    {
        ResolveTimerReference();
    }

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

    private void Update()
    {
        if(pickupAllowed && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;

            if (PickupText != null)
            {
                PickupText.gameObject.SetActive(true);
            }

            pickupAllowed = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = null;

            if (PickupText != null)
            {
                PickupText.gameObject.SetActive(false);
            }

            pickupAllowed = false;
        }
    }

    private void Pickup()
    {
        ResolveTimerReference();

        if (timer != null && timer.IsTimerRunning && playerTransform != null)
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

        if (!CollectedItemsState.TryCollect(itemId))
        {
            return;
        }

        if (PickupText != null)
        {
            PickupText.gameObject.SetActive(false);
        }

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
