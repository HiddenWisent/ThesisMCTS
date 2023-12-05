using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Coordinates : IComparable<Coordinates>, IEquatable<Coordinates>
{
    public int Team;
    public int Column;
    public int Row;

    public Coordinates(int team, int column, int row)
    {
        Team = team;
        Column = column;
        Row = row;
    }

    public Coordinates(int index)
    {
        Team = index / 6;
        if (Team == 1)
        {
            index -= 6;
        }
        Column = index / 3;
        if (Column == 1)
        {
            index -= 3;
        }
        Row = index;
    }
    public int Index { get { return Team * 6 + Column * 3 + Row; } }
    public int CompareTo(Coordinates other)
    {
        return Index - other.Index;
    }

    public bool Equals(Coordinates other)
    {
        if (Team == other.Team && Column == other.Column && Row == other.Row)
        {
            return true;
        }
        return false;
    }

    public List<int> GetEnemyNeighbours()
    {
        List<int> neighbours = new();
        for (int i = Mathf.Max(Row-1,0); i <= Mathf.Min(Row+1,2); i++)
        {
            neighbours.Add(i);
        }
        return neighbours;
    }

    public override string ToString()
    {
        return Team + "/" + Column + "/" + Row;
    }

}

public enum Range
{
    Meele,
    Ranged,
    All
}

public delegate void UnitCombatEventHandler(object sender, UnitCombatEventArgs e);

public class UnitCombatEventArgs : EventArgs
{
    public CombatActionType ActionType { get; set; }
    public UnitCombatEventArgs(CombatActionType actionType)
    {
        ActionType = actionType;
    }
}

public enum CombatActionType
{
    TurnStart,
    Attack,
    Defend,
    Wait,
    TakeDamage,
    TurnEnd,
    IsTargeted,
    Reset
}