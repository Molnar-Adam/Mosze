using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HPControllerTests
{
    private GameObject player;
    private PlayerHealth playerHealth;

    private GameObject controllerObj;
    private HPController controller;

    private GameObject heartPrefab;

    [SetUp]
    public void Setup()
    {
        player = new GameObject("Player");
        playerHealth = player.AddComponent<PlayerHealth>();

        heartPrefab = new GameObject("HeartPrefab");

        var image = heartPrefab.AddComponent<UnityEngine.UI.Image>();

        heartPrefab.AddComponent<HPHeart>();

        controllerObj = new GameObject("HPController");
        controller = controllerObj.AddComponent<HPController>();

        controller.playerHealth = playerHealth;
        controller.heartPrefab = heartPrefab;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(controllerObj);
        Object.DestroyImmediate(heartPrefab);
    }

    [UnityTest]
    public IEnumerator DrawHearts_CreatesCorrectAmount()
    {
        yield return null;

        int expected = (int)((playerHealth.MaxHealth / 2f) + (playerHealth.MaxHealth % 2));

        Assert.AreEqual(expected, controllerObj.transform.childCount,
            "Nem megfelelő számú heart jött létre!");
    }

    [UnityTest]
    public IEnumerator DrawHearts_UpdatesOnHealthChange()
    {
        yield return null;

        int initialCount = controllerObj.transform.childCount;

        var field = typeof(PlayerHealth)
            .GetField("lastDamageTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        field.SetValue(playerHealth, -100f);

        playerHealth.TakeDamage(2);

        yield return null;

        int newCount = controllerObj.transform.childCount;

        Assert.AreEqual(initialCount, newCount,
            "Heart count nem kéne változzon max health változás nélkül!");
    }

    [UnityTest]
    public IEnumerator ClearHearts_RemovesAll()
    {
        yield return null;

        controller.ClearHearts();

        yield return null;

        Assert.AreEqual(0, controllerObj.transform.childCount,
            "ClearHearts nem törölte az elemeket!");
    }

    [UnityTest]
    public IEnumerator CreateEmptyHeart_AddsOne()
    {
        yield return null;

        int before = controllerObj.transform.childCount;

        controller.CreateEmptyHeart();

        int after = controllerObj.transform.childCount;

        Assert.AreEqual(before + 1, after,
            "CreateEmptyHeart nem adott hozzá új heart-ot!");
    }
}