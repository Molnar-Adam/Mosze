using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int MaxHealth = 10;
    public int Health;
    public event Action OnHealthChanged;
    public event Action OnDied;

    private static int savedHealth;
    private static bool hasSavedHealth;

    private const float DAMAGE_COOLDOWN = 1f;
    private float lastDamageTime = 0f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        hasSavedHealth = false;
        savedHealth = 0;
    }

    private void Awake()
    {
        if (hasSavedHealth)
        {
            Health = Mathf.Clamp(savedHealth, 0, MaxHealth);
        }
        else
        {
            Health = MaxHealth;
            SaveRuntimeHealth();
        }

        OnHealthChanged?.Invoke();
    }

    public static void ResetSavedHealth()
    {
        hasSavedHealth = false;
        savedHealth = 0;
    }

    private void OnDestroy()
    {
        if (Health > 0)
        {
            SaveRuntimeHealth();
        }
    }
     
    public void TakeDamage(int damage)
    {
        if (Time.time - lastDamageTime < DAMAGE_COOLDOWN)
        {
            return;
        }

        lastDamageTime = Time.time;
       
       if(Health - damage > MaxHealth)
       {
        Health = 10;
        SaveRuntimeHealth();
        OnHealthChanged?.Invoke();
       }
       else{
        Health = Mathf.Max(Health - damage, 0);
        SaveRuntimeHealth();
        OnHealthChanged?.Invoke();
       }

        if (Health <= 0)
        {
            OnDied?.Invoke();
            ResetSavedHealth();
        }
    }

    private void SaveRuntimeHealth()
    {
        savedHealth = Health;
        hasSavedHealth = true;
    }
}
