//AI generált kód
//Prompt: Találj ki egy olyan megoldást, ahol a Cinemachine kamerák között tudok váltani egy trigger objecttel.
//Megoldás: A cinemachine kamerák prioritás szerint aktívak. A legnagyobb prioritású kamera aktív.
//A kód leviszi az eddigi aktív kamera prioritását(10) és megnöveli a kívánt kamera prioritását(100).

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