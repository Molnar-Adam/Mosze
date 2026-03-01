using UnityEngine;

public class RatMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;

    private Vector3 patrolPointAWorld;
    private Vector3 patrolPointBWorld;

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
    }

    void Update()
    {
        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
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
                if (Vector2.Distance(transform.position, patrolPointAWorld) < .2f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    patrolDestination = 1;
                }
            }

            if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPointBWorld, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPointBWorld) < .2f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    patrolDestination = 0;
                }
            }
        }
    }
}
