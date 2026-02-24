using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyDamageTests
{
    [UnityTest]
    public IEnumerator Enemy_Damages_Player_On_Collision()
    {
        // 1. SETUP
        GameObject playerObj = new GameObject("Player");
        playerObj.tag = "Player";

        var rb = playerObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep; // NE aludjon el

        var col = playerObj.AddComponent<BoxCollider2D>();
        col.size = new Vector2(1, 1); // Legyen mérete!
        col.isTrigger = false;

        var pHealth = playerObj.AddComponent<PlayerHealth>();
        var pMove = playerObj.AddComponent<PlayerMovement>();
        pHealth.Health = 10;
        pMove.KBTTime = 0.5f;

        // Ellenség létrehozása
        GameObject enemyObj = new GameObject("Enemy");
        var eCol = enemyObj.AddComponent<BoxCollider2D>();
        eCol.size = new Vector2(1, 1);
        eCol.isTrigger = false;

        var eDamage = enemyObj.AddComponent<EnemyDamage>();

        // Damage beállítása
        var field = typeof(EnemyDamage).GetField("damage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(eDamage, 2);

        eDamage.playerHealth = pHealth;
        eDamage.playerMovement = pMove;

        // 2. ACT
        enemyObj.transform.position = Vector3.zero;
        playerObj.transform.position = new Vector3(0.5f, 0, 0); // Legyenek átfedésben!

        // Kényszerítjük a fizikai motort, hogy észlelje az ütközést
        Physics2D.SyncTransforms();

        // Várunk egy kicsit többet
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForFixedUpdate();

        // 3. ASSERT
        Assert.AreEqual(8, pHealth.Health, "A HP nem csökkent! Az ütközés nem történt meg.");
        Assert.Greater(pMove.KBCounter, 0, "A Knockback nem aktiválódott!");

        Object.Destroy(playerObj);
        Object.Destroy(enemyObj);
    }
}