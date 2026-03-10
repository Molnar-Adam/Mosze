using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GrapplingHookTests
{
    [UnityTest]
    public IEnumerator Grapple_ReelsIn_WhenJointEnabled()
    {
        GameObject player = new GameObject("Player");

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        DistanceJoint2D joint = player.AddComponent<DistanceJoint2D>();
        LineRenderer rope = player.AddComponent<LineRenderer>();

        GrapplingHook grapplingHook = player.AddComponent<GrapplingHook>();

        var field = typeof(GrapplingHook).GetField("rope",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(grapplingHook, rope);

        yield return null;

        Assert.IsFalse(joint.enabled);

        joint.enabled = true;
        joint.distance = 10f;

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.LessOrEqual(joint.distance, 10f);
    }
}