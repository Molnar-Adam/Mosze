using UnityEngine;
using Unity.Cinemachine;

/// Szobák közötti kameraváltások kezelésére szolgáló osztály (Cinemachine használatával).
/// Trigger zónák alapján priorizálja a kamerákat.
public class CameraHandler : MonoBehaviour
{
    /// A jelenlegi szobához tartozó, aktiválandó Cinemachine kamera.
    public CinemachineCamera roomCamera;  

    /// Az előző szobához tartozó kamera, melyet ki kell kapcsolni.
    public CinemachineCamera disablecamera;  

    /// A kamera prioritása aktív állapotban.
    public int activePriority = 100;

    /// A kamera prioritása inaktív állapotban.
    public int inactivePriority = 10;

    /// Amikor a játékos belép a trigger zónába, megváltoztatja a kamerák prioritásait,
    /// ezáltal sima transition-t érve el a szobák között.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomCamera.Priority = activePriority;   
            disablecamera.Priority = inactivePriority;
        }
    }
}