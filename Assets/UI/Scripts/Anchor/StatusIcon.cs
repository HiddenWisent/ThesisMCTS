using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    private Image _image;
    private SpriteLibrary _sprites;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _sprites = GetComponent<SpriteLibrary>();
    }

    public void ChangeSprite(string spriteName)
    {
        Sprite sprite = _sprites.GetSprite("StatusIcons", spriteName);
        _image.sprite = sprite;
    }

}
