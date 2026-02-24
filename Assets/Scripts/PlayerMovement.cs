using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;

    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;

    public float KBForce;
    public float KBCounter;
    public float KBTTime;
    public bool KFromRight; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (groundCheck == null) return false;

        return Physics2D.OverlapCapsule(
            groundCheck.position,
            new Vector2(1f, 0.1f),
            CapsuleDirection2D.Horizontal,
            0f,
            groundLayer
        );
    }
}
