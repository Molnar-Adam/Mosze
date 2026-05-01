using UnityEngine;

/// A mozgó platform működéséért felelős osztály, amely két pont között ingázik.
public class MovingPlatform : MonoBehaviour
{
    /// Az első végpont (A pont).
    public Transform pointA;
    
    /// A második végpont (B pont).
    public Transform pointB;

    /// A platform sebessége.
    public float moveSpeed = 2f;

    private Vector3 nextPosition;

    /// Játék indításakor beállítja a célpontot a B pontra.
    void Start()
    {
        nextPosition = pointB.position;
    }

    /// Képkockánként simán mozgatja a platformot a célpont felé, elérés esetén megfordítja az irányt.
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if(transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    /// Ha a játékos rááll a platformra, annak gyermekévé teszi, így együtt mozognak.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    /// Amikor a játékos lelép a platformról, megszünteti a parent kapcsolatot.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
