using UnityEngine;

/// A patkány ellenség mozgását, járőrözését és a játékos követését kezelő osztály.
public class RatMovement : MonoBehaviour
{
    /// A járőrözési végpontokat tartalmazó tömb.
    public Transform[] patrolPoints;
    
    /// A patkány mozgási sebessége.
    public float moveSpeed;
    
    /// A jelenlegi járőrözési célpont indexe.
    public int patrolDestination;

    private Vector3 patrolPointAWorld;
    private Vector3 patrolPointBWorld;
    private float previousX;

    /// A játékos pozíciója a követéshez.
    public Transform playerTransform;
    
    /// Jelzi, hogy a patkány éppen követi-e a játékost.
    public bool isChasing;
    
    /// Az a távolság, amin belül a patkány elkezdi követni a játékost.
    public float chaseDistance;

    /// Az a távolság, ameddig a patkány hajlandó eltávolodni a járőrözés során.
    public float PartolDistance;

    /// Kezdéskor beállítja a járőrözési pontok világkoordinátáit.
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

    /// Képkockánként kezeli a játékos követését vagy a megadott pontok közötti járőrözést.
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
    /// Frissíti a patkány nézési irányát (forgatását) a mozgás iránya alapján.
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
