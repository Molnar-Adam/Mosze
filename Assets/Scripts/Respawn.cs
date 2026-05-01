using UnityEngine;

/// A játékos újraéledését, visszateleportálását és sebződését kezelő osztály.
public class Respawn : MonoBehaviour
{
    /// Az újraéledési pont pozíciója.
    [SerializeField] private Transform respawnLocation;
    
    /// A játékos objektumának azonosító címkéje.
    [SerializeField] private string playerTag = "Player";
    
    private Timer timer;

    /// A játékos életerejét kezelő komponens.
    public PlayerHealth playerHealth;
    
    /// A csapdába eséskor kapott sebzés mértéke.
    [SerializeField] int damage;

    /// Példányosításkor megkeresi és rögzíti a Timer komponenst.
    private void Awake()
    {
        ResolveTimerReference();
    }

    /// Ütközéskor sebzi a játékost és visszateleportálja az újraéledési pontra, illetve értesíti a timert.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
        {
            return;
        }

        ResolveTimerReference();

        if (timer != null && timer.IsTimerRunning)
        {
            timer.TriggerTimerEnd(collision.transform);
            playerHealth.TakeDamage(damage);
            return;
        }
        playerHealth.TakeDamage(damage);
        Transform playerTransform = collision.transform;
        playerTransform.position = respawnLocation.position;

        Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    /// Megkeresi a jelenetben lévő Timer komponenst, ha még nincs hivatkozás rá.
    private void ResolveTimerReference()
    {
        if (timer != null)
        {
            return;
        }

        timer = FindFirstObjectByType<Timer>();
    }
}
