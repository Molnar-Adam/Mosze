using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask hitMask = ~0;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        Vector2 delta = rb.linearVelocity * Time.fixedDeltaTime;
        float distance = delta.magnitude;
        if (distance <= 0f)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(rb.position, delta.normalized, distance + 0.05f, hitMask);
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            HandleHit(hit.collider.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject hitObject)
    {
        PlayerHealth playerHealth = hitObject.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = hitObject.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
