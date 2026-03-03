using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTests
{
    private const float KnockbackForce = 10f;

    [UnityTest]
    public IEnumerator Positive_Input_Moves_Player_To_The_Right()
    {
        // Arrange
        var player = CreatePlayer(out var rb, out var movement);
        rb.gravityScale = 0f;

        var startX = player.transform.position.x;

        // Act
        movement.HorizontalInput = 1f;

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.That(player.transform.position.x, Is.GreaterThan(startX));

        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator Knockback_From_Right_Pushes_Player_Left()
    {
        // Arrange
        var player = CreatePlayer(out var rb, out var movement);

        movement.KBForce = KnockbackForce;
        movement.KBCounter = 0.5f;
        movement.KFromRight = true;

        // Act
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.That(rb.linearVelocity.x, Is.LessThan(0));

        Object.Destroy(player);
    }

    private GameObject CreatePlayer(out Rigidbody2D rb, out PlayerMovement movement)
    {
        var player = new GameObject("Player");

        rb = player.AddComponent<Rigidbody2D>();
        movement = player.AddComponent<PlayerMovement>();

        return player;
    }
}