
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CombatUIToggler : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private bool _isVisibleDuringCombat;
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        UIEvents.instance.OnCombatStart += CombatStart;
        UIEvents.instance.OnCombatEnd += CombatEnd;
    }
    public void CombatStart()
    {
        TogglePanel(_isVisibleDuringCombat);
    }

    public void CombatEnd()
    {
        TogglePanel(!_isVisibleDuringCombat);
    }

    public void TogglePanel(bool isVisible)
    {
        if (isVisible)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
        }
        else
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
        }
    }
}
