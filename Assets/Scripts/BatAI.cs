using UnityEngine;

public class BatAI : MonoBehaviour
{
    public GameObject projectilePrefab; // Ide húzod majd be a lövedékedet
    public float fireRate = 2f; // 2 másodpercenként lõ

    void Start()
    {
        InvokeRepeating("Attack", 1f, fireRate);
    }

    void Attack()
    {
        // Itt váltasz át a támadó animációra az Animatorral
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
