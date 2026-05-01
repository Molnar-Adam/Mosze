using UnityEngine;
using TMPro;

/// A villanykapcsolót kezelő szkript, amellyel felkapcsolható az áram.
public class LightSwitch : MonoBehaviour
{
    /// A UI szöveg, ami jelzi, hogy interakcióba lehet lépni a kapcsolóval.
    [SerializeField] private TextMeshProUGUI InteractText;
    
    /// Jelzi, hogy a játékos a kapcsoló közelében van-e.
    private bool InteractAllowed = false;
    
    /// A játékos pozíciója.
    private Transform playerTransform;

    /// Kezdetben elrejti az interakciós feliratot.
    private void Start()
    {
        if (InteractText != null)
        {
            InteractText.gameObject.SetActive(false);
        }


    }

    /// Várja az "E" gomb lenyomását a felkapcsoláshoz, ha a zónán belül vagyunk.
    private void Update()
    {
        if(InteractAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PullLever();
        }
    }

    /// Ha a játékos belép a kapcsoló zónájába, engedélyezi a felkapcsolást és megjeleníti a feliratot.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;

            if (InteractText != null)
            {
                InteractText.gameObject.SetActive(true);
    /// Ha a játékos elhagyja a kapcsoló zónáját, eltünteti a feliratot és tiltja a műveletet.
            }

            InteractAllowed = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = null;

    /// Felkapcsolja az áramot (GameState.powerOn = true).
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

