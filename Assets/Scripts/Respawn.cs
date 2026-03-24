using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;
    [SerializeField] private string playerTag = "Player";
    private Timer timer;

    public PlayerHealth playerHealth;
    [SerializeField] int damage;

    private void Awake()
    {
        ResolveTimerReference();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
        {
            return;
        }

        ResolveTimerReference();

        if (timer != null && timer.IsTimerRunning)
        {
            timer.TriggerTimerEnd(collision.transform);
            playerHealth.TakeDamage(damage);
            return;
        }
        playerHealth.TakeDamage(damage);
        Transform playerTransform = collision.transform;
        playerTransform.position = respawnLocation.position;

        Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
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
