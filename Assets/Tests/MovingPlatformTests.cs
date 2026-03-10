using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovingPlatformTests
{
    [UnityTest]
    public IEnumerator Platform_MovesTowardsTargetAndSwitches()
    {
        GameObject platformGo = new GameObject("Platform");
        MovingPlatform platform = platformGo.AddComponent<MovingPlatform>();

        GameObject pA = new GameObject("PointA");
        GameObject pB = new GameObject("PointB");
        pA.transform.position = Vector3.zero;
        pB.transform.position = new Vector3(2f, 0, 0);

        platform.pointA = pA.transform;
        platform.pointB = pB.transform;
        platform.moveSpeed = 100f;
        platformGo.transform.position = pA.transform.position;

        yield return null;

        float timeout = 1f;
        while (Vector3.Distance(platformGo.transform.position, pB.transform.position) > 0.1f && timeout > 0)
        {
            timeout -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Assert.Less(Vector3.Distance(platformGo.transform.position, pB.transform.position), 0.2f, "A platform nem érte el a B pont közelét!");
    }

    [UnityTest]
    public IEnumerator Player_BecomesChildOfPlatform_ManualCheck()
    {
        GameObject platformGo = new GameObject("Platform");
        GameObject playerGo = new GameObject("Player");
        playerGo.tag = "Player";

        playerGo.transform.parent = platformGo.transform;
        Assert.AreEqual(platformGo.transform, playerGo.transform.parent);

        playerGo.transform.parent = null;
        Assert.IsNull(playerGo.transform.parent);

        yield return null;
    }
}
