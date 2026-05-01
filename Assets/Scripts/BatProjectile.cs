using UnityEngine;

/// A denevér által kilőtt lövedék viselkedését és fizikáját szabályozó osztály.
/// Ütközéskor (vagy trigger esetén) sebzést okoz a játékosnak.
public class BatProjectile : MonoBehaviour
{
    /// A játékosnak okozott sebzés mértéke.
    [SerializeField] private int damage = 1;

    /// A lövedék maximális élettartama rásodpercben.
    [SerializeField] private float lifeTime = 5f;

    /// A layermask, amivel a lövedék képes ütközni.
    [SerializeField] private LayerMask hitMask = ~0;

    /// A lövedék fizikai testét irányító Rigidbody2D komponens.
    private Rigidbody2D rb;

    /// A példányosításkor elmenti a Rigidbody2D referenciát.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// Kezdéskor beütemezi a lövedék megsemmisülését a megadott élettartam után.
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    /// Fizikai frissítési ciklus. Raycast segítségével is detektálja az ütközéseket a gyors mozgás miatt.
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

    /// Akkor hívódik meg, ha egy másik trigger collider ér hozzá a lövedékhez.
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }

    /// Akkor hívódik meg, ha a lövedék szilárd testtel ütközik.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    /// Lekezeli a találatot: megpróbál PlayerHealth komponenst találni a célponton, 
    /// ha sikerül, sebzést oszt ki, majd maga a lövedék megsemmisül.
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
