using UnityEngine;
using UnityEngine.UI;

public class LoadImageUI : MonoBehaviour
{
    [SerializeField] private RawImage _image;

    public void LoadSprite(Texture2D sprite)
    {
        _image.texture = sprite;
    }
}
