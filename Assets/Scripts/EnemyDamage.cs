using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage;
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Csak ha a Player-hez ér hozzá
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

            if (health != null) // Csak akkor fut le, ha megtalálta a szkriptet
            {
                health.TakeDamage(1); // Itt adja le a sebzést
            }
            else
            {
                Debug.LogWarning("A Player-en nincs PlayerHealth szkript!");
            }
        }
    }


}
