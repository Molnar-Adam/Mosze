using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float fallDelay = 1f;
    [SerializeField] float respawnDelay = 5f;

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private RigidbodyType2D initialBodyType;
    private bool isTriggered;

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

    IEnumerator FallAndRespawnRoutine()
    {
        isTriggered = true;

        yield return new WaitForSeconds(fallDelay);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

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
