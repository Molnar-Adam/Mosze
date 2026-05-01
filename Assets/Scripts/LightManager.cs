using UnityEngine;
using UnityEngine.Rendering.Universal;

/// Kezeli a globális és a játékoshoz kötött fények be- és kikapcsolását a játékállapot (powerOn) alapján.
public class LightManager : MonoBehaviour
{
    /// A pályát bevilágító globális 2D fény.
    public Light2D globalLight;
    
    /// A játékos körüli 2D fény.
    public Light2D playerLight;

    /// Induláskor beállítja a fényeket a GameState.powerOn értékétől függően.
    void Start()
    {
        if (GameState.powerOn)
        {
            globalLight.enabled = true;
            playerLight.enabled = false;
        }
        else
        {
            globalLight.enabled = false;
            playerLight.enabled = true;
        }
    }
}