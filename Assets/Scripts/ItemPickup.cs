using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PickupText;
    [SerializeField] private string itemId;

    private bool pickupAllowed;

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
            if (PickupText != null)
            {
                PickupText.gameObject.SetActive(false);
            }

            pickupAllowed = false;
        }
    }

    private void Pickup()
    {
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
}
