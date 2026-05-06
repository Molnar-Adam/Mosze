using UnityEngine;

/// A grappling hook működését kezelő osztály.
public class GrapplingHook : MonoBehaviour
{
    /// A távolság, amire a csáklya behúzza a játékost.
    [SerializeField] float grappleLength = 1f;
    
    /// A kötelet vizuálisan megjelenítő LineRenderer.
    [SerializeField] LineRenderer rope;
    
    /// A maximális távolság a horog rögzítéséhez.
    [SerializeField] float maxGrappleDistance = 10f;
    
    /// A behúzás sebessége.
    [SerializeField] float grappleReelSpeed = 8f;

    /// A falak ellenőrzéséhez használt réteg.
    [SerializeField] LayerMask groundLayer;

    /// A rögzítési pont pozíciója.
    private Vector3 grapplePoint;
    
    /// A csatlakozást biztosító 2D joint.
    private DistanceJoint2D joint;

    /// Inicializálja a komponenst és elrejti a kötelet.
    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    /// Gombnyomásra megkeresi a legközelebbi Hook-ot és kilövi a csáklyát.
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameObject[] hooks = GameObject.FindGameObjectsWithTag("Hook");
            GameObject closestHook = null;
            float closestDistance = maxGrappleDistance;

            foreach (var hook in hooks)
            {
                float distance = Vector2.Distance(transform.position, hook.transform.position);
                if (distance <= closestDistance)
                {
                    Vector2 direction = (hook.transform.position - transform.position).normalized;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);

                    if (hit.collider == null)
                    {
                        closestDistance = distance;
                        closestHook = hook;
                    }
                }
            }

            if(closestHook != null)
            {
                grapplePoint = closestHook.transform.position;
                grapplePoint.z = 0;

                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                joint.distance = closestDistance;
                
                rope.SetPosition(0, grapplePoint);
    /// Fizikai frissítés, amely fokozatosan húzza a játékost a rögzítési pont felé.
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.L))
        {
            joint.enabled = false;
            rope.enabled = false;
        }

        if(rope.enabled == true)
        {
            rope.SetPosition(1, transform.position);
        }
    }

    void FixedUpdate()
    {
        if (!joint.enabled)
        {
            return;
        }

        // Ellenőrizzük megszakadt-e a rálátás a rögzítési pontra (van-e fal közöttük)
        Vector2 direction = (grapplePoint - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, grapplePoint);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);

        if (hit.collider != null)
        {
            joint.enabled = false;
            rope.enabled = false;
            return;
        }

        float reelStep = grappleReelSpeed * Time.fixedDeltaTime;
        joint.distance = Mathf.MoveTowards(joint.distance, grappleLength, reelStep);
    }
}

