using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[Serializable]
public class UnitStats : IComparable<UnitStats>
{

    public Coordinates Coordinates { get; private set; }
    public int Damage { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; set; }
    public int Speed { get; private set; }
    public Range Range { get; private set; }
    public bool IsDefending { get; set; }
    public bool IsWaiting { get; set; }
    public bool IsDead { get { return CurrentHealth == 0; } }
    public void SetCoordinates(Coordinates coordinates)
    {
        Coordinates = coordinates;
    }
    public UnitStats(Coordinates coordinates, int damage, int maxHealth, int currentHealth, int speed, Range range)
    {
        Coordinates = coordinates;
        Damage = damage;
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Speed = speed;
        Range = range;
    }
    public UnitStats(UnitStats other)
    {
        Coordinates = new(other.Coordinates.Index);
        Damage = other.Damage;
        MaxHealth = other.MaxHealth;
        CurrentHealth = other.CurrentHealth;
        Speed = other.Speed;
        Range = other.Range;
        IsDefending = other.IsDefending;
        IsWaiting = other.IsWaiting;
    }
    public void SetStats(int damage, int maxHealth, int speed, Range range)
    {
        Damage = damage;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Speed = speed;
        Range = range;
    }

    public void CopyStats(UnitStats stats)
    {
        Coordinates = stats.Coordinates;
        Damage = stats.Damage;
        MaxHealth = stats.MaxHealth;
        CurrentHealth = stats.CurrentHealth;
        Speed = stats.Speed;
        Range = stats.Range;
    }

    public int CompareTo(UnitStats other)
    {
        return other.Speed.CompareTo(Speed);
    }
}
