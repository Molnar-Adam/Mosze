using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class HPControllerTests
{
    [UnityTest]
    public IEnumerator HPController_DrawsCorrectNumberOfHearts_BasedOnMaxHealth()
    {
        GameObject controllerGo = new GameObject("HPController");
        HPController controller = controllerGo.AddComponent<HPController>();

        GameObject playerGo = new GameObject("Player");
        PlayerHealth health = playerGo.AddComponent<PlayerHealth>();
        controller.playerHealth = health;

        GameObject heartPrefab = new GameObject("HeartPrefab");
        heartPrefab.AddComponent<Image>();
        HPHeart heartComp = heartPrefab.AddComponent<HPHeart>();

        heartComp.fullHeart = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        heartComp.HalfHeart = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        heartComp.emptyHeart = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);

        controller.heartPrefab = heartPrefab;

        health.MaxHealth = 6;
        health.Health = 6;

        controller.DrawHearts();

        yield return null;

        Assert.AreEqual(3, controllerGo.transform.childCount, "Nem jött létre a megfelelő számú szív objektum.");

        Object.Destroy(controllerGo);
        Object.Destroy(playerGo);
        Object.Destroy(heartPrefab);
    }
}
