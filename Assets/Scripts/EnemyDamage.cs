using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage;
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.KBCounter = playerMovement.KBTTime;
            if (collision.transform.position.x <= transform.transform.position.x)
            {
                playerMovement.KFromRight = true;
            }
            else
            {
                playerMovement.KFromRight = false;
            }
            playerHealth.TakeDamage(damage);
        }
    }


}
