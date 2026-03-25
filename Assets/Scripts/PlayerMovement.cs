using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;

    [Header("Ground Check Settings")]
    [SerializeField] LayerMask[] groundLayers;
    [SerializeField] float groundProbeDistance = 0.08f;
    [SerializeField] float minGroundNormalY = 0.6f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[8];
    private float horizontalInput;

    public float KBForce;
    public float KBCounter;
    public float KBTTime;
    public bool KFromRight; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if(KBCounter <= 0)
        {
            rb.linearVelocity = new Vector2(HorizontalInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            if(KFromRight == true)
            {
                rb.linearVelocity = new Vector2(-KBForce, KBForce);
            }
            if (KFromRight == false)
            {
                rb.linearVelocity = new Vector2(KBForce, KBForce);
            }

            KBCounter -= Time.deltaTime;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        HorizontalInput = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public float HorizontalInput
    {
        get => horizontalInput;
        set => horizontalInput = value;
    }

    private bool IsGrounded()
    {
        if (playerCollider == null || groundLayers == null || groundLayers.Length == 0) return false;

        int combinedGroundMask = 0;
        foreach (LayerMask groundLayer in groundLayers)
        {
            combinedGroundMask |= groundLayer.value;
        }

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(combinedGroundMask);
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;

        int hitCount = playerCollider.Cast(Vector2.down, contactFilter, groundHits, groundProbeDistance);
        for (int i = 0; i < hitCount; i++)
        {
            if (groundHits[i].normal.y >= minGroundNormalY)
            {
                return true;
            }
        }

        return false;
    }
}
