using UnityEngine;
using UnityEngine.InputSystem;

/// A játékos mozgását és ugrását (fizikai erőkkel) vezérlő osztály.
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    /// A játékos vízszintes mozgási sebessége.
    [SerializeField] float moveSpeed = 5f;
    
    /// A játékos ugrásának ereje.
    [SerializeField] float jumpForce = 10f;

    [Header("Ground Check Settings")]
    /// Azok a rétegek, amelyek "talajnak" minősülnek az ugráshoz.
    [SerializeField] LayerMask[] groundLayers;
    
    /// A talajérzékelő sugár hossza lefelé.
    [SerializeField] float groundProbeDistance = 0.08f;
    
    /// A minimális normál Vektor Y tengelyi érték, amely még talajnak számít.
    [SerializeField] float minGroundNormalY = 0.6f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[8];
    private float horizontalInput;

    /// A visszalökődés ereje (Knockback Force).
    public float KBForce;
    
    /// A visszalökődés számlálója, eddig hat rá a fizikai taszítás.
    public float KBCounter;
    
    /// A visszalökődés maximális időtartama.
    public float KBTTime;
    
    /// Igaz, ha jobbról kapott sebzést a játékos.
    public bool KFromRight; 

    /// Játék indításakor eltárolja a fizikai komponenseket (Rigidbody2D, Collider).
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    /// Fizikai frissítési ciklus. Ellenőrzi a sebesség változását és a visszalökődés (Knockback) állapotát.
    
    // AI Generált kód
    // Prompt: A knockback felülírja a playerinputokat, találj rá valami megoldást
    // Megoldás: Knockback egyszer triggerel és utána a fizika oldja meg a mozgást. Eddig a lineáris sebesség volt állítva. 
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

    /// Az új Input System Action callbackje a vízszintes mozgás beolvasására.
    public void Move(InputAction.CallbackContext context)
    {
        HorizontalInput = context.ReadValue<Vector2>().x;
    }

    /// Az új Input System Action callbackje a gombnyomás és talajellenőrzés után történő ugrásra.
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    /// Lekérhető és beállítható a vízszintes bemenet aktuális értéke.
    public float HorizontalInput
    {
        get => horizontalInput;
        set => horizontalInput = value;
    }

    /// Megvizsgálja, hogy a játékos a földön áll-e sugárvetéssel (Cast lefelé a collider aljától).
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
