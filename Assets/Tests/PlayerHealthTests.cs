using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerHealthTests
{
    [UnityTest]
    public IEnumerator Health_Decreases_When_Taking_Damage()
    {
        // ARRANGE - Előkészítés
        GameObject playerObj = new GameObject();
        PlayerHealth health = playerObj.AddComponent<PlayerHealth>();

        // Mivel a Start() nem fut le azonnal az AddComponent után, 
        // manuálisan beállítjuk a kezdő értéket a teszthez.
        health.Health = 10;

        // ACT - Művelet
        health.TakeDamage(3);

        // ASSERT - Ellenőrzés
        Assert.AreEqual(7, health.Health, "A HP-nak 7-nek kellene lennie 3 sebzés után!");

        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Player_Is_Destroyed_When_Health_Reaches_Zero()
    {
        // ARRANGE
        GameObject playerObj = new GameObject("TestPlayer");
        PlayerHealth health = playerObj.AddComponent<PlayerHealth>();
        health.Health = 5;

        // ACT
        health.TakeDamage(5);

        // Várnunk kell egy frame-et, mert a Destroy() csak a frame végén hajtódik végre
        yield return null;

        // ASSERT
        // Megpróbáljuk megkeresni az objektumot. Ha null, akkor sikeresen törlődött.
        Assert.IsTrue(playerObj == null, "A játékos objektumnak meg kellett volna semmisülnie!");
    }
}
