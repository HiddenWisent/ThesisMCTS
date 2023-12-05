using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageToChoose : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Image _image;
    private ImageChooserPanel _chooserPanel;


    private void Awake()
    {
        _chooserPanel = GetComponentInParent<ImageChooserPanel>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerId == -1)
        {
            _chooserPanel.ChooseImage(_image.sprite);
        }
    }
}
