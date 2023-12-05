using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDropSlot : MonoBehaviour, IDropHandler
{
    private UnitAnchor _anchor;
    private void Awake()
    {
        _anchor = GetComponentInParent<UnitAnchor>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerId == -1) 
        {
            DraggableImage image = eventData.pointerDrag.GetComponent<DraggableImage>();
            if (image != null && image.IsDragOn && image.IsCombatOff)
            {
                if (image.Anchor == _anchor)
                {
                    return;
                }
                Unit unit = image.Anchor.Unit;
                if (_anchor.Unit == null)
                {
                    _anchor.AddUnit(unit, image.Anchor.Image.sprite);
                    image.Anchor.MoveUnit();
                }
                else
                {
                    _anchor.SwapUnits(image.Anchor);
                }
            }
        }
    }
}
