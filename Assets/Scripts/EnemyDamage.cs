using UnityEngine;

/// Ez az osztály felelős az ellenségek által a játékosnak okozott sebzésért, 
/// és a knockback (hátralökődés) triggereléséért.
public class EnemyDamage : MonoBehaviour
{
    /// A sebzés mértéke, amit az ellenség okoz a játékosnak.
    [SerializeField] int damage;
    
    ///A PlayerHealth-re vonatkozó hivatkozás
    public PlayerHealth playerHealth;

    /// A PlayerMovement-re vonatkozó hivatkozás a knockbackhez.
    public PlayerMovement playerMovement;

    /// Akkor hívódik meg, amikor a player érintkezik az ellenfél Collider-ével 
    /// Meghívja a TakeDaage() scriptet és gondoskodik a knokbackről
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(1); 
                PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.KBCounter = movement.KBTTime;
                    movement.KFromRight = collision.transform.position.x <= transform.position.x;
                }            }
            else
            {
                Debug.LogWarning("A Player-en nincs PlayerHealth szkript!");
            }
        }
    }


}
