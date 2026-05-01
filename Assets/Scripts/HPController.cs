using System.Collections.Generic;
using UnityEngine;

/// A játékos életerejét megjelenítő UI kontroller.
public class HPController : MonoBehaviour
{
    /// A szív UI elemet reprezentáló prefab.
    public GameObject heartPrefab;
    
    /// >A játékos Health komponensére mutató referencia.
    public PlayerHealth playerHealth;
    
    /// A jelenleg létrehozott szívek listája.
    List<HPHeart> hearts = new List<HPHeart>();

    /// Feliratkozik a játékos életerő-változás eseményére.
    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += DrawHearts;
        }
    }

    /// Leiratkozik a játékos életerő-változás eseményéről.
    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= DrawHearts;
        }
    }

    /// Induláskor kirajzolja a kezdő szíveket.
    private void Start()
    {
        DrawHearts();
    }

    /// Törli a korábbi szíveket, majd az aktuális max életerő alapján újakat hoz létre és állít be.
    public void DrawHearts()
    {
        ClearHearts();
        float maxHealthRemainder = playerHealth.MaxHealth % 2;
        int heartsToMake = (int)((playerHealth.MaxHealth / 2) + maxHealthRemainder);
        for(int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.Health - (i*2),0,2);
            hearts[i].SetHeartImage((HeartState)heartStatusRemainder);
        }

    }

    /// Létrehoz és elment egy teljesen üres új szívet a listába.
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform);
        HPHeart heartComponent = newHeart.GetComponent<HPHeart>();
        heartComponent.SetHeartImage(HeartState.Empty);
        hearts.Add(heartComponent);
    }

    /// Eltávolítja az összes szívet a képernyőről és kiüríti a listát.
    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HPHeart>();
    }
}
