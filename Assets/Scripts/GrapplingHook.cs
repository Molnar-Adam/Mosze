using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] float grappleLength;
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] LineRenderer rope;
    [SerializeField] float maxGrappleDistance = 10f;
    [SerializeField] float downwardGrappleSpeedMultiplier = 0.3f;
    [SerializeField] float grappleReelSpeed = 8f;

    private Vector3 grapplePoint;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;
    private float targetGrappleDistance;

    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mouseWorldPos - transform.position).normalized;
            float distanceToMouse = Vector2.Distance(transform.position, mouseWorldPos);

            RaycastHit2D hit = Physics2D.Raycast(
                origin: (Vector2)transform.position,
                direction: directionToMouse,
                distance: distanceToMouse,
                layerMask: grappleLayer
            );

            if(hit.collider == null)
            {
                return;
            }

            grapplePoint = hit.point;
            grapplePoint.z = 0;

            float currentDistanceToPoint = Vector2.Distance(transform.position, grapplePoint);
            if (currentDistanceToPoint > maxGrappleDistance)
            {
                return;
            }

            joint.connectedAnchor = grapplePoint;
            joint.enabled = true;
            targetGrappleDistance = grappleLength;
            joint.distance = currentDistanceToPoint;
            rope.SetPosition(0, grapplePoint);
            rope.SetPosition(1, transform.position);
            rope.enabled = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            rope.enabled = false;
        }

        if(rope.enabled == true)
        {
            rope.SetPosition(1,transform.position);
        }
    }

    void FixedUpdate()
    {
        if (!joint.enabled)
        {
            return;
        }

        float reelStep = grappleReelSpeed * downwardGrappleSpeedMultiplier * Time.fixedDeltaTime;
        joint.distance = Mathf.MoveTowards(joint.distance, targetGrappleDistance, reelStep);
    }
}

