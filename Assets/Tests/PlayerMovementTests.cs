using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTests
{
    [UnityTest]
    public IEnumerator Player_Moves_Right_When_Input_Is_Positive()
    {
        // ARRANGE (Előkészítés)
        GameObject playerObj = new GameObject();
        Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
        // Beállítjuk, hogy ne essen le a végtelenbe a teszt alatt
        rb.gravityScale = 0;

        PlayerMovement movement = playerObj.AddComponent<PlayerMovement>();

        float startX = playerObj.transform.position.x;

        // ACT (Művelet)
        // Szimuláljuk, mintha a joystick jobbra lenne tolva
        movement.HorizontalInput = 1f;

        // Várunk 2-3 fizikai frissítést (FixedUpdate)
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // ASSERT (Ellenőrzés)
        Assert.Greater(playerObj.transform.position.x, startX, "A játékosnak jobbra kellett volna mozdulnia!");

        Object.Destroy(playerObj);
    }

    [UnityTest]
    public IEnumerator Knockback_Moves_Player_In_Correct_Direction()
    {
        // ARRANGE
        GameObject playerObj = new GameObject();
        Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
        PlayerMovement movement = playerObj.AddComponent<PlayerMovement>();

        movement.KBForce = 10f;
        movement.KBCounter = 0.5f; // Aktiváljuk a knockback-et
        movement.KFromRight = true; // Jobbról érkezik az ütés -> balra kell repülni

        // ACT
        yield return new WaitForFixedUpdate();

        // ASSERT
        // Ha jobbról lökik, a sebességének (velocity) negatívnak kell lennie X tengelyen
        Assert.Less(rb.linearVelocity.x, 0, "Knockback esetén balra kellene repülni!");

        Object.Destroy(playerObj);
    }
}