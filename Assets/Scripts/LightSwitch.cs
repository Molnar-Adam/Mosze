using UnityEngine;
using TMPro;


public class LightSwitch : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI InteractText;
    private bool InteractAllowed = false;
    private Transform playerTransform;

    private void Start()
    {
        if (InteractText != null)
        {
            InteractText.gameObject.SetActive(false);
        }


    }

    private void Update()
    {
        if(InteractAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PullLever();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;

            if (InteractText != null)
            {
                InteractText.gameObject.SetActive(true);
            }

            InteractAllowed = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = null;

            if (InteractText != null)
            {
                InteractText.gameObject.SetActive(false);
            }

            InteractAllowed = false;
        }
    }

    public void PullLever()
    {
        GameState.powerOn = true;
        Debug.Log("Power ON");
    }

    

}

