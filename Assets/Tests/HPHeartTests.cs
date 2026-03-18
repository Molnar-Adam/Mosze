using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class HPHeartTests
{
    [UnityTest]
    public IEnumerator HPHeart_SetsCorrectSprite_WhenStateChanges()
    {
        GameObject heartGo = new GameObject("TestHeart");
        Image image = heartGo.AddComponent<Image>();
        HPHeart hpHeart = heartGo.AddComponent<HPHeart>();

        Sprite full = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        Sprite half = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        Sprite empty = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);

        hpHeart.fullHeart = full;
        hpHeart.HalfHeart = half;
        hpHeart.emptyHeart = empty;

        yield return null;

        hpHeart.SetHeartImage(HeartState.Full);
        Assert.AreEqual(full, image.sprite, "Full állapotban nem a megfelelő sprite-ot kapta meg!");

        hpHeart.SetHeartImage(HeartState.Half);
        Assert.AreEqual(half, image.sprite, "Half állapotban nem a megfelelő sprite-ot kapta meg!");

        hpHeart.SetHeartImage(HeartState.Empty);
        Assert.AreEqual(empty, image.sprite, "Empty állapotban nem a megfelelő sprite-ot kapta meg!");

        Object.Destroy(heartGo);
    }
}