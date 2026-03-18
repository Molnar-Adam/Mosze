using UnityEngine;
using Unity.Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public CinemachineCamera roomCamera;  
    public CinemachineCamera disablecamera;  // Cameras to deactivate
    public int activePriority = 100;
    public int inactivePriority = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomCamera.Priority = activePriority;
                disablecamera.Priority = inactivePriority;
        }
    }
}