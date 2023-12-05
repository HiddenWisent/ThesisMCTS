using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action OnCombatStart;
    public event Action OnCombatEnd;

    public void CombatStart()
    {
        OnCombatStart?.Invoke();
    }

    public void CombatEnd()
    {
        OnCombatEnd?.Invoke();
    }
    public event Action<Unit, int> OnUnitPressed;
    public event Action<int> OnUnitReleased;
    public event Action<Unit, int> OnUnitClicked;

    public void PressUnit(Unit unit, int button)
    {
        OnUnitPressed?.Invoke(unit, button);
    }

    public void ReleaseUnit(int button)
    {
        OnUnitReleased?.Invoke(button);
    }

    public void ClickUnit(Unit unit, int button)
    {
        OnUnitClicked?.Invoke(unit, button);
    }
}
