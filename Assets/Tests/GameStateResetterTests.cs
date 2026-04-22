using NUnit.Framework;
using UnityEngine;

public class GameStateResetterTests
{
    [SetUp]
    public void Setup()
    {

        PlayerPrefs.SetInt("TestKey", 123);

        PlayerHealth.ResetSavedHealth();
        var player = new GameObject();
        var health = player.AddComponent<PlayerHealth>();
        health.Health = 5;

        CollectedItemsState.TryCollect("Map1_Key");

        Time.timeScale = 0f;
    }

    [Test]
    public void ResetGameState_ResetsAllSystems()
    {
        // Act
        GameStateResetter.ResetGameState();

        // ---------------- ASSERT ----------------

        Assert.AreEqual(0, PlayerPrefs.GetInt("TestKey", 0),
            "PlayerPrefs nem lett törölve!");

        Assert.AreEqual(0, CollectedItemsState.CollectedCount,
            "CollectedItems nem lett resetelve!");

        var newPlayer = new GameObject();
        var newHealth = newPlayer.AddComponent<PlayerHealth>();

        Assert.AreEqual(newHealth.MaxHealth, newHealth.Health,
            "PlayerHealth nem lett resetelve!");

        Assert.AreEqual(1f, Time.timeScale,
            "Time.timeScale nem lett visszaállítva!");

        Object.DestroyImmediate(newPlayer);
    }
}