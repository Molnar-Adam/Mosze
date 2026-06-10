using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// A játék hangerő-beállításainak kezelése.
/// A hangerő értékét PlayerPrefs-ben tárolja, így a beállítások a játék újraindítása után is megmaradnak.
public class VolumeSettings : MonoBehaviour
{
    /// Az AudioMixer, amelynek hangerő paraméterét módosítjuk.
    [SerializeField] private AudioMixer myMixer;

    /// A hangerőt szabályozó UI Slider.
    [SerializeField] private Slider mySlider;

    /// A jelenet indulásakor betölti a korábban elmentett hangerőt, vagy alapértelmezett értékkel inicializálja azt.
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

    /// Beállítja a hangerőt az AudioMixerben a Slider aktuális értéke alapján, majd elmenti azt a PlayerPrefs-be.

    public void setVolume()
    {
        // A slider aktuális értéke (0.0 - 1.0).
        float volume = mySlider.value;

        // A lineáris hangerőértéket decibel skálára alakítjuk, mivel az AudioMixer ezt a formátumot használja.
        myMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);

        // A hangerő elmentése a következő indításhoz.
        PlayerPrefs.SetFloat("volume", volume);
    }

    /// Betölti az elmentett hangerőértéket, majd alkalmazza azt az AudioMixerre. 
    private void LoadVolume()
    {
        // Korábban mentett hangerő beolvasása.
        mySlider.value = PlayerPrefs.GetFloat("volume");

        // A betöltött érték alkalmazása.
        setVolume();
    }
}