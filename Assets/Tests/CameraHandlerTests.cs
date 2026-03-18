using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Cinemachine;

public class CameraHandlerTests
{
    [UnityTest]
    public IEnumerator PlayerEnteringTrigger_ChangesCameraPriorities()
    {
        GameObject trigger = new GameObject("CameraTrigger");
        BoxCollider2D collider = trigger.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        CameraHandler handler = trigger.AddComponent<CameraHandler>();

        GameObject camObj1 = new GameObject("RoomCamera");
        CinemachineCamera roomCamera = camObj1.AddComponent<CinemachineCamera>();

        GameObject camObj2 = new GameObject("DisableCamera");
        CinemachineCamera disableCamera = camObj2.AddComponent<CinemachineCamera>();

        handler.roomCamera = roomCamera;
        handler.disablecamera = disableCamera;

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<BoxCollider2D>();

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        player.transform.position = new Vector2(-5, 0);

        yield return null;

        player.transform.position = trigger.transform.position;

        yield return null;
        yield return null;

        Assert.AreEqual(handler.activePriority, roomCamera.Priority.Value);
        Assert.AreEqual(handler.inactivePriority, disableCamera.Priority.Value);
    }
}