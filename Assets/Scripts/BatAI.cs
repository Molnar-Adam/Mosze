using UnityEngine;

public class BatAI : MonoBehaviour
{
    public GameObject projectilePrefab; // Ide húzod majd be a lövedékedet
    public float fireRate = 2f;         // 2 másodpercenként lõ

    private Animator anim;              // Ezt a sort adtuk hozzá: ez tárolja az animátort

    void Start()
    {
        // Megkeressük a denevéren lévõ Animator komponenst
        anim = GetComponent<Animator>();

        InvokeRepeating("Attack", 1f, fireRate);
    }

    void Attack()
    {
        // Most már mûködni fog, mert tudja, mi az az anim
        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }

        // Létrehozzuk a lövedéket
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
