using UnityEngine;
using UnityEngine.UI;

public class HPHeart : MonoBehaviour
{
    public Sprite fullHeart, HalfHeart, emptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

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

public enum HeartState
{
    Empty = 0,
    Half = 1,

    Full= 2,
}
