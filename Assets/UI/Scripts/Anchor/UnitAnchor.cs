using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitAnchor : MonoBehaviour
{
    [SerializeField]
    private Coordinates _coordinates;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private StatusIcon _statusIcon;
    public Image Image { get { return _image; } }
    [SerializeField]
    private TextMeshProUGUI _healthNumber;
    [SerializeField]
    private RemoveUnitButton _removeUnitButton;
    public Unit Unit { get; private set; }
    public Coordinates Coordinates { get { return _coordinates; } }

    public void AddUnit(Unit unit, Sprite sprite)
    {
        Unit = unit;
        Unit.name = Unit.Stats.Range.ToString() + " " + _coordinates.Index;
        _image.sprite = sprite;
        CanvasGroup canvasGroup = _image.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        ChangeHealthNumber();
        unit.Stats.SetCoordinates(_coordinates);
        _removeUnitButton.gameObject.SetActive(true);
        unit.Events.UnitAction += ReactToUnitAction;
    }

    public void MoveUnit()
    {
        Unit.Events.UnitAction -= ReactToUnitAction;
        Unit = null;
        CanvasGroup canvasGroup = _image.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        _removeUnitButton.gameObject.SetActive(false);
        _healthNumber.text = "";
    }
    public void SwapUnits(UnitAnchor other)
    {
        Unit tempUnit = Unit;
        MoveUnit();
        AddUnit(other.Unit, other.Unit.Sprite);
        other.MoveUnit();
        other.AddUnit(tempUnit, tempUnit.Sprite);
    }

    internal void RemoveUnit()
    {
        Unit tempUnit = Unit;
        MoveUnit();
        Destroy(tempUnit.gameObject);
    }

    private void ReactToUnitAction(object sender, UnitCombatEventArgs e)
    {
        switch (e.ActionType)
        {
            case CombatActionType.TurnStart:
                ChangeBackgroundColor(200, 150, 30);
                _statusIcon.ChangeSprite("Empty");
                break;
            case CombatActionType.Attack:
                break;
            case CombatActionType.Defend:
                _statusIcon.ChangeSprite("Defend");
                break;
            case CombatActionType.Wait:
                _statusIcon.ChangeSprite("Wait");
                break;
            case CombatActionType.TakeDamage:
                ChangeHealthNumber();
                break;
            case CombatActionType.TurnEnd:
                ChangeBackgroundColor(252, 255, 235);
                ChangeHealthNumber();
                break;
            case CombatActionType.IsTargeted:
                ChangeBackgroundColor(0, 190, 0);
                break;
            case CombatActionType.Reset:
                ChangeBackgroundColor(252, 255, 235);
                ChangeHealthNumber();
                _statusIcon.ChangeSprite("Empty");
                break;
            default:
                break;
        }
    }

    private void ChangeHealthNumber()
    {
        _healthNumber.text = Unit.Stats.CurrentHealth + "/" + Unit.Stats.MaxHealth;
    }

    private void ChangeBackgroundColor(byte r, byte g, byte b)
    {
        GetComponent<Image>().color = new Color32(r, g, b, 255);
    }
    private void OnDestroy()
    {
        if (Unit != null)
        {
            Unit.Events.UnitAction -= ReactToUnitAction;
        }
    }
}
