using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerLight;

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