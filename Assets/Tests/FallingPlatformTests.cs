using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FallingPlatformTests
{
    [UnityTest]
    public IEnumerator PlayerCollision_MakesPlatformFall_AndRespawn()
    {
        // Platform létrehozása
        GameObject platform = new GameObject("Platform");
        platform.transform.position = Vector3.zero;

        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();

        Rigidbody2D rb = platform.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        FallingPlatform fallingPlatform = platform.AddComponent<FallingPlatform>();

        GameObject player = new GameObject("Player");
        player.tag = "Player";

        BoxCollider2D playerCollider = player.AddComponent<BoxCollider2D>();

        Rigidbody2D playerRb = player.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 0;

        player.transform.position = new Vector2(0, 1);

        yield return null;

        player.transform.position = new Vector2(0, 0);

        yield return new WaitForSeconds(1.2f);

        Assert.AreEqual(RigidbodyType2D.Dynamic, rb.bodyType);

        yield return new WaitForSeconds(5.2f);

        Assert.AreEqual(Vector3.zero, platform.transform.position);
    }
}