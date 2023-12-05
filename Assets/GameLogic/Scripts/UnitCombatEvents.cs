using System;
using UnityEngine;
public class UnitCombatEvents
{
    public UnitStats Stats;

    public UnitCombatEvents(UnitStats stats)
    {
        Stats = stats;
    }

    public event EventHandler<UnitCombatEventArgs> UnitAction;

    public void TurnStart()
    {
        Stats.IsDefending = false;
        OnUnitAction(CombatActionType.TurnStart);
    }
    public void TakeDamage(int damage)
    {
        if (Stats.IsDefending)
        {
            damage /= 2;
        }
        Stats.CurrentHealth -= Mathf.Clamp(damage, 0, Stats.CurrentHealth);
        OnUnitAction(CombatActionType.TakeDamage);
    }

    public void Attack(Unit target)
    {
        Stats.IsWaiting = false;
        int damage = Stats.Damage;
        target.Events.TakeDamage(damage);
        OnUnitAction(CombatActionType.Attack);
        TurnEnd();
    }

    public void Defend()
    {
        Stats.IsWaiting = false;
        Stats.IsDefending = true;
        OnUnitAction(CombatActionType.Defend);
        TurnEnd();
    }

    public void Wait()
    {
        TurnEnd();
        Stats.IsWaiting = true;
        OnUnitAction(CombatActionType.Wait);
    }

    public void TurnEnd()
    {
        OnUnitAction(CombatActionType.TurnEnd);
    }

    public void Reset()
    {
        Stats.CurrentHealth = Stats.MaxHealth;
        Stats.IsDefending = false;
        Stats.IsWaiting = false;
        OnUnitAction(CombatActionType.Reset);
    }

    public void IsTargeted()
    {
        OnUnitAction(CombatActionType.IsTargeted);
    }
    protected virtual void OnUnitAction(CombatActionType action)
    {
        EventHandler<UnitCombatEventArgs> handler = UnitAction;

        handler?.Invoke(this, new UnitCombatEventArgs(action));
    }
}
