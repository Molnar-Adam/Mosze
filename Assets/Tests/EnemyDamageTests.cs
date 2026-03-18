using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class EnemyDamageTests
{
    private const int StartingHealth = 10;
    private const int DamageAmount = 2;

    [UnityTest]
    public IEnumerator Collision_With_Enemy_Reduces_Player_Health_And_Triggers_Knockback()
    {
        var player = CreatePlayer(out var health, out var movement);
        var enemy = CreateEnemy(health, movement);
        var damageComponent = enemy.GetComponent<EnemyDamage>();

        enemy.transform.position = Vector3.zero;
        player.transform.position = new Vector3(0.5f, 0f, 0f);

        MethodInfo onCollisionMethod = typeof(EnemyDamage).GetMethod("OnCollisionEnter2D",
            BindingFlags.NonPublic | BindingFlags.Instance);

        Collision2D fakeCollision = new Collision2D();

        yield return new WaitForFixedUpdate();

        player.transform.position = new Vector3(100f, 100f, 0f);
        yield return null;

        Assert.AreEqual(StartingHealth - DamageAmount, health.Health,
            "A sebzés mértéke nem megfelelő! (Lehet, hogy még mindig többször sebezett)");
        Assert.Greater(movement.KBCounter, 0, "A Knockback nem aktiválódott!");

        Object.Destroy(player);
        Object.Destroy(enemy);
    }

    private GameObject CreatePlayer(out PlayerHealth health, out PlayerMovement movement)
    {
        var player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<Rigidbody2D>().isKinematic = true;
        player.AddComponent<BoxCollider2D>();

        health = player.AddComponent<PlayerHealth>();
        movement = player.AddComponent<PlayerMovement>();

        health.Health = StartingHealth;
        movement.KBTTime = 0.5f;

        return player;
    }

    private GameObject CreateEnemy(PlayerHealth health, PlayerMovement movement)
    {
        var enemy = new GameObject("Enemy");
        enemy.AddComponent<BoxCollider2D>();
        var damage = enemy.AddComponent<EnemyDamage>();

        var field = typeof(EnemyDamage).GetField("damage",
            BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(damage, DamageAmount);

        damage.playerHealth = health;
        damage.playerMovement = movement;

        return enemy;
    }
}