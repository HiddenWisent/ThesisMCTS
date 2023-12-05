using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveUnitButton : MonoBehaviour, IPointerClickHandler
{
    private UnitAnchor _anchor;

    private void Awake()
    {
        _anchor = GetComponentInParent<UnitAnchor>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _anchor.RemoveUnit();
    }
}
