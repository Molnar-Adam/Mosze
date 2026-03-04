using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RatMovementTests
{
    private const float ChaseDistance = 5f;

    [UnityTest]
    public IEnumerator Rat_Starts_Chasing_When_Player_Enters_Chase_Range()
    {
        // Arrange
        var rat = CreateRat(out var ratObj, out var player);

        rat.chaseDistance = ChaseDistance;
        rat.moveSpeed = 0f;
        rat.isChasing = false;

        // Player kívül a hatótávon
        player.position = new Vector3(10f, 0f, 0f);
        yield return null;

        Assert.That(rat.isChasing, Is.False);

        // Act – player belép a hatótávba
        player.position = new Vector3(3f, 0f, 0f);
        yield return null;

        // Assert
        Assert.That(rat.isChasing, Is.True);

        Cleanup(ratObj, player.gameObject);
    }

    [UnityTest]
    public IEnumerator Rat_Switches_To_Next_Patrol_Point_When_Reaching_Current()
    {
        // Arrange
        var rat = CreateRat(out var ratObj, out _);

        var p1 = new GameObject("Point1");
        var p2 = new GameObject("Point2");

        p1.transform.position = Vector3.zero;
        p2.transform.position = new Vector3(10f, 0f, 0f);

        rat.patrolPoints = new[] { p1.transform, p2.transform };
        rat.patrolDestination = 0;
        rat.isChasing = false;
        rat.moveSpeed = 100f;

        ratObj.transform.position = p1.transform.position;

        // Act
        yield return null;

        // Assert
        Assert.That(rat.patrolDestination, Is.EqualTo(1));

        Cleanup(ratObj, p1, p2);
    }

    private RatMovement CreateRat(out GameObject ratObj, out Transform playerTransform)
    {
        ratObj = new GameObject("Rat");
        ratObj.SetActive(false);

        var rat = ratObj.AddComponent<RatMovement>();

        var dummyPoint1 = new GameObject("P1");
        var dummyPoint2 = new GameObject("P2");
        rat.patrolPoints = new[] { dummyPoint1.transform, dummyPoint2.transform };

        var player = new GameObject("Player");
        playerTransform = player.transform;

        rat.playerTransform = playerTransform;

        ratObj.SetActive(true);

        return rat;
    }

    private void Cleanup(params GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
                Object.Destroy(obj);
        }
    }
}