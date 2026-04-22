using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class DeathPanelControllerTests
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

        // Player
        player = new GameObject("Player");
        player.tag = "Player";
        playerHealth = player.AddComponent<PlayerHealth>();

        // Panel
        panel = new GameObject("DeathPanel");

        // Controller
        controllerObj = new GameObject("Controller");
        controller = controllerObj.AddComponent<DeathPanelController>();

        var flags = BindingFlags.NonPublic | BindingFlags.Instance;

        typeof(DeathPanelController)
            .GetField("deathPanel", flags)
            .SetValue(controller, panel);

        typeof(DeathPanelController)
            .GetField("playerHealth", flags)
            .SetValue(controller, playerHealth);
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;

        Object.DestroyImmediate(player);
        Object.DestroyImmediate(panel);
        Object.DestroyImmediate(controllerObj);
    }

    [UnityTest]
    public IEnumerator Death_ShowsPanel_And_PausesGame()
    {
        yield return null;

        var field = typeof(PlayerHealth)
            .GetField("lastDamageTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        field.SetValue(playerHealth, -100f);

        // Act
        playerHealth.TakeDamage(playerHealth.MaxHealth);

        yield return null;

        // Assert
        Assert.IsTrue(panel.activeSelf, "A death panel nem aktiválódott!");
        Assert.IsTrue(DeathPanelController.IsDeathScreenActive,
            "A static flag nem lett beállítva!");
        Assert.AreEqual(0f, Time.timeScale,
            "A játék nem lett megállítva!");
    }

    [UnityTest]
    public IEnumerator DeathPanel_ShownOnlyOnce()
    {
        yield return null;

        playerHealth.TakeDamage(playerHealth.MaxHealth);
        yield return null;

        bool firstState = panel.activeSelf;

        playerHealth.TakeDamage(1);
        yield return null;

        // Assert
        Assert.IsTrue(firstState, "Panel nem jelent meg elsőre!");
        Assert.IsTrue(panel.activeSelf, "Panel eltűnt második hívás után!");
    }

    [UnityTest]
    public IEnumerator Awake_DisablesPanel()
    {
        var panel = new GameObject("Panel");
        panel.SetActive(true);

        var controllerObj = new GameObject("Controller");

        var controller = controllerObj.AddComponent<DeathPanelController>();

        var field = typeof(DeathPanelController)
            .GetField("deathPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        field.SetValue(controller, panel);

        var method = typeof(DeathPanelController)
            .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        method.Invoke(controller, null);

        yield return null;

        Assert.IsFalse(panel.activeSelf, "Awake nem kapcsolta ki a panelt!");

        Object.Destroy(panel);
        Object.Destroy(controllerObj);
    }
}
