
using UnityEngine;

public class EnableEndScreen : MonoBehaviour
{
    [SerializeField] private GameObject objectToEnable;

    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }
        }
    }
}