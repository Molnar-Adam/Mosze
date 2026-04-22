using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class FallingPlatformTests
{
    private GameObject platformObj;
    private FallingPlatform platform;
    private Rigidbody2D rb;

    [SetUp]
    public void Setup()
    {
        platformObj = new GameObject("Platform");

        rb = platformObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        platformObj.AddComponent<BoxCollider2D>();
        platform = platformObj.AddComponent<FallingPlatform>();

        var flags = BindingFlags.NonPublic | BindingFlags.Instance;

        typeof(FallingPlatform)
            .GetField("fallDelay", flags)
            .SetValue(platform, 0.1f);

        typeof(FallingPlatform)
            .GetField("respawnDelay", flags)
            .SetValue(platform, 0.1f);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(platformObj);
    }

    [UnityTest]
    public IEnumerator Platform_Falls_And_Respawns()
    {
        Vector3 startPos = platformObj.transform.position;

        StartCoroutineDirect();

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(RigidbodyType2D.Dynamic, rb.bodyType,
            "Platform nem esett le!");

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(RigidbodyType2D.Kinematic, rb.bodyType,
            "Platform nem állt vissza!");

        Assert.AreEqual(startPos, platformObj.transform.position,
            "Pozíció nem lett visszaállítva!");
    }

    [UnityTest]
    public IEnumerator Platform_Triggers_OnlyOnce()
    {
        StartCoroutineDirect();
        StartCoroutineDirect();

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(RigidbodyType2D.Dynamic, rb.bodyType,
            "Többször triggerelődött!");
    }

    private void StartCoroutineDirect()
    {
        var method = typeof(FallingPlatform)
            .GetMethod("FallAndRespawnRoutine",
                BindingFlags.NonPublic | BindingFlags.Instance);

        var enumerator = (IEnumerator)method.Invoke(platform, null);
        platform.StartCoroutine(enumerator);
    }
}