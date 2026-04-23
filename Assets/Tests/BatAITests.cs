using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BatAITests
{
    [UnityTest]
    public IEnumerator BatAI_ShootsProjectile_AfterInterval()
    {
        // ---------------- ARRANGE ----------------
        GameObject bat = new GameObject("Bat");
        BatAI batAI = bat.AddComponent<BatAI>();

        GameObject projectilePrefab = new GameObject("ProjectilePrefab");

        Rigidbody2D rbPrefab = projectilePrefab.AddComponent<Rigidbody2D>();
        rbPrefab.gravityScale = 0;

        GameObject shootPointObj = new GameObject("ShootPoint");
        shootPointObj.transform.position = new Vector3(1, 2, 0);

        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

        typeof(BatAI).GetField("projectilePrefab", flags).SetValue(batAI, projectilePrefab);
        typeof(BatAI).GetField("shootPoint", flags).SetValue(batAI, shootPointObj.transform);
        typeof(BatAI).GetField("shootInterval", flags).SetValue(batAI, 0.1f);
        typeof(BatAI).GetField("projectileSpeed", flags).SetValue(batAI, 6f);

        int initialCount = GameObject.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None).Length;

        // ---------------- ACT ----------------
        yield return new WaitForSeconds(0.2f);

        Rigidbody2D[] bodies = GameObject.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

        Rigidbody2D newProjectile = null;

        foreach (var body in bodies)
        {
            if (body.gameObject.name.Contains("ProjectilePrefab(Clone)"))
            {
                newProjectile = body;
                break;
            }
        }

        Assert.IsNotNull(newProjectile, "Nem jött létre új projectile!");

        yield return new WaitForFixedUpdate();

        // ---------------- ASSERT ----------------
        Assert.AreEqual(Vector2.down * 6f, newProjectile.linearVelocity);

        Assert.IsNotNull(newProjectile.GetComponent<BatProjectile>(),
            "Hiányzik a BatProjectile komponens!");
    }
}