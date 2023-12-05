using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageChooser : MonoBehaviour, IPointerDownHandler
{ 
    private Image _image;
    [SerializeField]
    private ImageChooserPanel _chooserPanel;

    public void OnPointerDown(PointerEventData eventData)
    {
        _chooserPanel.OpenPanel();
    }

    internal void ChangeSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
}
