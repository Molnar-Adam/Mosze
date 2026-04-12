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
        Health = Mathf.Max(Health - damage, 0);
        SaveRuntimeHealth();
        OnHealthChanged?.Invoke();

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
