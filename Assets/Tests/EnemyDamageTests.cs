using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyDamageTests
{
    private const int StartingHealth = 10;
    private const int DamageAmount = 2;

    [UnityTest]
    public IEnumerator Collision_With_Enemy_Reduces_Player_Health_And_Triggers_Knockback()
    {
        // Arrange
        var player = CreatePlayer(out var health, out var movement);
        var enemy = CreateEnemy(health, movement);

        enemy.transform.position = Vector3.zero;
        player.transform.position = new Vector3(0.5f, 0f, 0f);

        Physics2D.SyncTransforms();

        // Act
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.That(health.Health, Is.EqualTo(StartingHealth - DamageAmount));
        Assert.That(movement.KBCounter, Is.GreaterThan(0));

        Object.Destroy(player);
        Object.Destroy(enemy);
    }

    private GameObject CreatePlayer(out PlayerHealth health, out PlayerMovement movement)
    {
        var player = new GameObject("Player");
        player.tag = "Player";

        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        var collider = player.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;

        health = player.AddComponent<PlayerHealth>();
        movement = player.AddComponent<PlayerMovement>();

        health.Health = StartingHealth;
        movement.KBTTime = 0.5f;

        return player;
    }

    private GameObject CreateEnemy(PlayerHealth health, PlayerMovement movement)
    {
        var enemy = new GameObject("Enemy");

        var collider = enemy.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;

        var damage = enemy.AddComponent<EnemyDamage>();

        var field = typeof(EnemyDamage)
            .GetField("damage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        field.SetValue(damage, DamageAmount);

        damage.playerHealth = health;
        damage.playerMovement = movement;

        return enemy;
    }
}