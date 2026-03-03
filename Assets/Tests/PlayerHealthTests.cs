using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerHealthTests
{
    private const int DefaultHealth = 10;

    [UnityTest]
    public IEnumerator TakeDamage_Reduces_Health_By_DamageAmount()
    {
        // Arrange
        var player = CreatePlayer(DefaultHealth);
        var health = player.GetComponent<PlayerHealth>();

        // Act
        health.TakeDamage(3);

        // Assert
        Assert.That(health.Health, Is.EqualTo(DefaultHealth - 3));

        Object.Destroy(player);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Player_Is_Destroyed_When_Health_Reaches_Zero()
    {
        // Arrange
        var player = CreatePlayer(5);
        var health = player.GetComponent<PlayerHealth>();

        // Act
        health.TakeDamage(5);
        yield return null; // Destroy a frame végén fut le

        // Assert
        Assert.That(player == null, Is.True);
    }

    private GameObject CreatePlayer(int startingHealth)
    {
        var player = new GameObject("Player");
        var health = player.AddComponent<PlayerHealth>();
        health.Health = startingHealth;

        return player;
    }
}
