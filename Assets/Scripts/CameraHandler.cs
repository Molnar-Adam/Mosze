using UnityEngine;
using Unity.Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public CinemachineCamera roomCamera;  
    public CinemachineCamera disablecamera;
    public int activePriority = 100;
    public int inactivePriority = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (roomCamera == null)
            {
                Debug.LogError("roomCamera nincs beállítva ezen: " + gameObject.name);
                return;
            }

            roomCamera.Priority = activePriority;
        }
    }
}