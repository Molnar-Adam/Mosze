using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int MaxHealth = 10;
    public int Health;

    private void Start()
    {
        Health = MaxHealth;
    }
     
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
