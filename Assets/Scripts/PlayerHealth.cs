using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int MaxHealth = 10;
    public int Health;
    public event Action OnHealthChanged;

    private void Awake()
    {
        Health = MaxHealth;
        OnHealthChanged?.Invoke();
    }
     
    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(Health - damage, 0);
        OnHealthChanged?.Invoke();

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
