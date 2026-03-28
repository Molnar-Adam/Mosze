using UnityEngine;
using Unity.Cinemachine;

public class BatAI : MonoBehaviour
{
    [Header("Combat")]
    public GameObject projectilePrefab;
    public float fireRate = 2f;
    public float alertDelay = 0.5f;

    [Header("Camera")]
    public CinemachineCamera roomCamera;

    private Animator anim;
    private bool isAlerted = false;
    private bool isAttacking = false;
    private float cameraCheckTimer = 0f;
    private float alertTimer = 0f;

    private enum BatState { Calm, Alert, Attacking }
    private BatState currentState = BatState.Calm;

    void Start()
    {
        anim = GetComponent<Animator>();
        SetState(BatState.Calm);
    }

    void Update()
    {
        // Check if room camera became active
        if (!isAlerted && roomCamera != null)
        {
            cameraCheckTimer += Time.deltaTime;
            if (cameraCheckTimer >= 0.1f)
            {
                if (roomCamera.Priority >= 100)
                {
                    SetState(BatState.Alert);
                    alertTimer = alertDelay;
                }
                cameraCheckTimer = 0f;
            }
        }

        // Transition from Alert to Attacking after delay
        if (currentState == BatState.Alert)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0f)
            {
                SetState(BatState.Attacking);
            }
        }
    }

    void SetState(BatState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case BatState.Calm:
                anim.SetTrigger("Calm");
                CancelInvoke("FireProjectile");
                isAlerted = false;
                isAttacking = false;
                break;

            case BatState.Alert:
                anim.SetTrigger("Alert");
                CancelInvoke("FireProjectile");
                isAlerted = true;
                isAttacking = false;
                break;

            case BatState.Attacking:
                anim.SetTrigger("Attack");
                isAlerted = true;
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0.2f, fireRate);
                break;
        }
    }

    void FireProjectile()
    {
        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
