using UnityEngine;
using UnityEngine.UI;

/// Egyetlen szív UI vizuális megjelenését vezérlő szkript.
public class HPHeart : MonoBehaviour
{
    /// A teljes, fél és üres szív sprite-jai.
    public Sprite fullHeart, HalfHeart, emptyHeart;
    
    /// A szívhez tartozó Image komponens.
    Image heartImage;

    /// Betöltéskor lekéri az Image komponenst.
    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    /// Frissíti a szív vizuális állapotát a megadott enum alapján.
    public void SetHeartImage(HeartState state)
    {
        switch (state)
        {
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartState.Half:
                heartImage.sprite = HalfHeart;
                break;
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
        }
    }
}

/// Egy szív állapotát reprezentáló enum.
public enum HeartState
{
    /// Üres szív (0 HP).
    Empty = 0,
    /// Fél szív (1 HP).
    Half = 1,
    /// Tele szív (2 HP).
    Full= 2,
}
