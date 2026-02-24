using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RatMovementTests
{
    // Ebbe is kell a [UnityTest], mert várnunk kell az Update()-re (yield return null)
    [UnityTest]
    public IEnumerator Rat_Starts_Chasing_When_Player_Is_Close()
    {
        // 1. SETUP - Inaktív objektum létrehozása
        GameObject ratObj = new GameObject("Rat");
        ratObj.SetActive(false); // Megállítjuk az Update-et, amíg nem állítunk be mindent!

        RatMovement rat = ratObj.AddComponent<RatMovement>();

        // Létrehozunk dummy pontokat, hogy ne legyen NullReference
        GameObject p1 = new GameObject("P1");
        GameObject p2 = new GameObject("P2");
        rat.patrolPoints = new Transform[] { p1.transform, p2.transform };

        GameObject playerObj = new GameObject("Player");
        rat.playerTransform = playerObj.transform;

        rat.chaseDistance = 5f;
        rat.isChasing = false;
        rat.moveSpeed = 0f;

        // Most, hogy minden be van állítva, aktiváljuk
        ratObj.SetActive(true);

        // 2. ACT
        playerObj.transform.position = new Vector3(10f, 0, 0);
        yield return null;
        Assert.IsFalse(rat.isChasing);

        playerObj.transform.position = new Vector3(3f, 0, 0);
        yield return null;

        // 3. ASSERT
        Assert.IsTrue(rat.isChasing);

        Object.Destroy(ratObj);
        Object.Destroy(playerObj);
        Object.Destroy(p1);
        Object.Destroy(p2);
    }

    [UnityTest]
    public IEnumerator Rat_Switches_Patrol_Destination_At_Point()
    {
        // ARRANGE
        GameObject ratObj = new GameObject("Rat");
        RatMovement rat = ratObj.AddComponent<RatMovement>();

        GameObject p1 = new GameObject("Point1");
        GameObject p2 = new GameObject("Point2");
        p1.transform.position = Vector3.zero;
        p2.transform.position = new Vector3(10f, 0, 0);

        rat.patrolPoints = new Transform[] { p1.transform, p2.transform };
        rat.playerTransform = new GameObject("DummyPlayer").transform; // Ne legyen null
        rat.moveSpeed = 100f;
        rat.patrolDestination = 0;
        rat.isChasing = false;

        // ACT
        ratObj.transform.position = p1.transform.position; // Pontosan rárakjuk az 1-es pontra
        yield return null; // Megvárjuk, amíg az Update érzékeli a távolságot

        // ASSERT
        Assert.AreEqual(1, rat.patrolDestination, "A patkánynak váltania kellett volna a 2. célpontra!");

        // CLEANUP
        Object.Destroy(ratObj);
        Object.Destroy(p1);
        Object.Destroy(p2);
    }
}