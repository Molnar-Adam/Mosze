using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class DeathIntegrationTests
{
    private GameObject player;
    private PlayerHealth playerHealth;

    private GameObject panel;
    private GameObject controllerObj;
    private DeathPanelController controller;


    [SetUp]
    public void Setup()
    {
        Time.timeScale = 1f;

        PlayerHealth.ResetSavedHealth();

        typeof(DeathPanelController)
            .GetMethod(
                "ResetRuntimeState",
                BindingFlags.NonPublic | BindingFlags.Static)
            ?.Invoke(null, null);


        player = new GameObject("Player");
        player.tag = "Player";

        playerHealth = player.AddComponent<PlayerHealth>();


        panel = new GameObject("DeathPanel");
        panel.SetActive(true);


        controllerObj = new GameObject("Controller");

        controller = controllerObj.AddComponent<DeathPanelController>();

        var flags = BindingFlags.NonPublic | BindingFlags.Instance;

        typeof(DeathPanelController)
            .GetField("deathPanel", flags)
            .SetValue(controller, panel);

        typeof(DeathPanelController)
            .GetField("playerHealth", flags)
            .SetValue(controller, playerHealth);

        typeof(DeathPanelController)
            .GetMethod("Awake", flags)
            .Invoke(controller, null);

        typeof(DeathPanelController)
            .GetMethod("OnEnable", flags)
            .Invoke(controller, null);
    }


    [UnityTest]
    public IEnumerator PlayerDies_ShowsPanel_And_PausesGame()
    {
        yield return null;

        playerHealth.Health = playerHealth.MaxHealth;

        var field = typeof(PlayerHealth)
            .GetField(
                "lastDamageTime",
                BindingFlags.NonPublic | BindingFlags.Instance);

        field.SetValue(playerHealth, -100f);

        playerHealth.TakeDamage(playerHealth.MaxHealth);

        yield return null;
        yield return null;

        Assert.IsTrue(
            panel.activeSelf,
            "A death panel nem jelent meg!");

        Assert.IsTrue(
            DeathPanelController.IsDeathScreenActive,
            "A static flag nem lett beállítva!");

        Assert.AreEqual(
            0f,
            Time.timeScale,
            "A játék nem állt meg!");
    }


    [UnityTest]
    public IEnumerator DeathPanel_Awake_DisablesPanelInitially()
    {
        yield return null;

        Assert.IsFalse(
            panel.activeSelf,
            "Awake nem kapcsolta ki a panelt!");
    }



    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Time.timeScale = 1f;

        PlayerHealth.ResetSavedHealth();
        CollectedItemsState.ResetProgress();

        Object.Destroy(player);
        Object.Destroy(panel);
        Object.Destroy(controllerObj);

        yield return null;
    }
}