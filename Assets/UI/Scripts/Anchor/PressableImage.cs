using UnityEngine;
using UnityEngine.EventSystems;

public class PressableImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private UnitAnchor _anchor;
    private DraggableImage _draggableImage;
    private void Awake()
    {
        _anchor = GetComponentInParent<UnitAnchor>();
        _draggableImage = GetComponent<DraggableImage>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_anchor.Unit != null)
        {
            int buttonId = eventData.pointerId;
            if(buttonId == -2) 
            {
                _draggableImage.TurnDragOff();
            }
            UIEvents.instance.PressUnit(_anchor.Unit, buttonId);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_anchor.Unit != null)
        {
            UIEvents.instance.ReleaseUnit(eventData.pointerId);
            _draggableImage.TurnDragOn();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_anchor.Unit != null)
        {
            UIEvents.instance.ClickUnit(_anchor.Unit, eventData.pointerId);
        }
    }
}
