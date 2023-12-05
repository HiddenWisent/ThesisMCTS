using System;
using UnityEngine;

public class Unit : MonoBehaviour, IComparable<Unit>
{
    public UnitStats Stats { get; private set; }
    public UnitCombatEvents Events { get; private set; }
    public Sprite Sprite { get; set; }

    public int CompareTo(Unit other)
    {
        return other.Stats.Speed.CompareTo(Stats.Speed);
    }

    internal void SetStats(Coordinates coordinates, int damage, int health, int speed, Range range)
    {
        Stats = new(coordinates, damage, health, health, speed, range);
        Events = new UnitCombatEvents(Stats);
    }

    public void CopyStats(UnitStats stats)
    {
        Stats.CopyStats(stats);
        Events = new UnitCombatEvents(Stats);
    }
}