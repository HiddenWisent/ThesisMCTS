using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _rectTransform;
    private Transform _originalParent;
    public UnitAnchor Anchor { get; private set; }
    private Image _image;
    private CanvasGroup _canvasGroup;
    private Vector2 _startPosition;
    public bool IsDragOn { get; private set; }
    public bool IsCombatOff { get; private set; }
    private void Awake()
    {
        IsDragOn = true;
        IsCombatOff = true;
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
        Anchor = GetComponentInParent<UnitAnchor>();
    }

    private void Start()
    {
        UIEvents.instance.OnCombatStart += CombatOn;
        UIEvents.instance.OnCombatEnd += CombatOff;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1 && IsDragOn && IsCombatOff)
        {
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            _image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1 && IsDragOn && IsCombatOff) 
        {
            _rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsDragOn && IsCombatOff)
        {
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(1);
            _rectTransform.anchoredPosition = _startPosition;
            _image.raycastTarget = true;
        }
    }

    public void TurnDragOff()
    {
        IsDragOn = false;
    }
    public void TurnDragOn()
    {
        IsDragOn = true;
    }

    public void CombatOff()
    {
        IsCombatOff = true;
    }
    public void CombatOn()
    {
        IsCombatOff = false;
    }

    private void OnDestroy()
    {
        if(gameObject != null)
        {
            UIEvents.instance.OnCombatStart -= CombatOn;
            UIEvents.instance.OnCombatEnd -= CombatOff;
        }
    }
}
