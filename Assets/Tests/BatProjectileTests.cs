using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class BatProjectileTests
{
    [UnityTest]
    public IEnumerator Projectile_HitsPlayer_DealsDamage_And_DestroysItself()
    {
        // ---------------- ARRANGE ----------------
        var player = new GameObject("Player");
        player.tag = "Player";

        var playerHealth = player.AddComponent<PlayerHealth>();
        player.AddComponent<BoxCollider2D>();
        var playerRb = player.AddComponent<Rigidbody2D>();
        playerRb.bodyType = RigidbodyType2D.Kinematic;

        yield return null;

        int startHealth = playerHealth.Health;

        var field = typeof(PlayerHealth)
            .GetField("lastDamageTime", BindingFlags.NonPublic | BindingFlags.Instance);

        field.SetValue(playerHealth, -100f);

        var projectile = new GameObject("Projectile");
        var rb = projectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;

        projectile.AddComponent<BoxCollider2D>().isTrigger = true;
        var projScript = projectile.AddComponent<BatProjectile>();

        projectile.transform.position = Vector3.zero;
        player.transform.position = Vector3.zero;

        // ---------------- ACT ----------------
        yield return new WaitForFixedUpdate();
        yield return null;

        // ---------------- ASSERT ----------------
        Assert.Less(playerHealth.Health, startHealth,
            "A projectile nem sebezte a játékost!");

        Assert.IsTrue(projectile == null || projectile.Equals(null),
            "A projectile nem semmisült meg!");

        Object.Destroy(player);
    }
}
