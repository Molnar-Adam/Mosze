using UnityEngine;

public class RatMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;

    private Vector3 patrolPointAWorld;
    private Vector3 patrolPointBWorld;
    private float previousX;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;

    public float PartolDistance;

    void Start()
    {
        if (patrolPoints == null || patrolPoints.Length < 2)
        {
            return;
        }

        patrolPointAWorld = patrolPoints[0].position;
        patrolPointBWorld = patrolPoints[1].position;
        previousX = transform.position.x;
    }

    void Update()
    {
        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                UpdateRotationBasedOnDirection();
                if (Vector2.Distance(transform.position, patrolPointAWorld) < .2f)
                {
                    isChasing = false;
                }
                if (Vector2.Distance(transform.position, patrolPointBWorld) < .2f)
                {
                    isChasing = false;
                }
            }
            if (transform.position.x < playerTransform.position.x)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                UpdateRotationBasedOnDirection();
                if (Vector2.Distance(transform.position, patrolPointAWorld) < .2f)
                {
                    isChasing = false;
                }
                if (Vector2.Distance(transform.position, patrolPointBWorld) < .2f)
                {
                    isChasing = false;
                }
            }
             if(Vector2.Distance(transform.position, playerTransform.position) > PartolDistance)
            {
                isChasing = false;
            }

        }
        else
        {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
            }
            if (patrolDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPointAWorld, moveSpeed * Time.deltaTime);
                UpdateRotationBasedOnDirection();
                if (Vector2.Distance(transform.position, patrolPointAWorld) < .2f)
                {
                    patrolDestination = 1;
                }
            }

            if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPointBWorld, moveSpeed * Time.deltaTime);
                UpdateRotationBasedOnDirection();
                if (Vector2.Distance(transform.position, patrolPointBWorld) < .2f)
                {
                    patrolDestination = 0;
                }
            }
        }
    }

    private void UpdateRotationBasedOnDirection()
    {
        if (transform.position.x > previousX)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (transform.position.x < previousX)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        previousX = transform.position.x;
    }
}
