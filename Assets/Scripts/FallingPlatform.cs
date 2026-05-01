using UnityEngine;
using System.Collections;

/// Egy platformot kezelő script, ami megadott időzítéssel
/// leesik, ha a játékos ráugrik, majd később visszaspawnol a helyére.
public class FallingPlatform : MonoBehaviour
{
    /// Az idő másodpercben, amennyit vár az esés megkezdése előtt, miután ráugrottak.
    [SerializeField] float fallDelay = 1f;

    /// Az idő másodpercben az esés megkezdésétől számítva, mire újra megjelenik.
    [SerializeField] float respawnDelay = 5f;

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private RigidbodyType2D initialBodyType;
    private bool isTriggered;

    /// Lekéri a komponenseket betöltéskor és elmenti a platform eredeti pozícióját is.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (rb != null)
        {
            initialBodyType = rb.bodyType;
        }
    }

    /// Amikor egy szilárd test hozzáér a platformhoz.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered)
        {
            return;
        }

        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }

        StartCoroutine(FallAndRespawnRoutine());
    }

    /// Várakozást indító Coroutine, ami kivárja az esést, Dynamic-ra állítja a RigidBody-t, 
    /// majd egy újabb várás után visszahelyezi Kinematic-ba és eredeti pozícióba.
    IEnumerator FallAndRespawnRoutine()
    {
        isTriggered = true;

        // Várakozás leesés előtt
        yield return new WaitForSeconds(fallDelay);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Várakozás az újjászületés előtt
        yield return new WaitForSeconds(respawnDelay);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;

        if (rb != null)
        {
            rb.bodyType = initialBodyType;
        }

        if (platformCollider != null)
        {
            platformCollider.enabled = true;
        }

        isTriggered = false;
    }
}
