using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
   [SerializeField] private AudioMixer myMixer;
   [SerializeField] private Slider mySlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            LoadVolume();
        }
        else
        {
        setVolume();
        }
    }

    public void setVolume()
{
    float volume = mySlider.value;
    myMixer.SetFloat("Volume",Mathf.Log10(volume)*20);
    PlayerPrefs.SetFloat("volume",volume);

}

private void LoadVolume()
    {
        mySlider.value = PlayerPrefs.GetFloat("volume");

        setVolume();
    }

}

