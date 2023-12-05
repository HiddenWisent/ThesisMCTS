using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardState
{
    public UnitStats[] Stats { get; private set; } = new UnitStats[12];
    public UnitsOrder Order { get; private set; }
    public int CurrentUnitIndex { get; private set; }
    public List<int> CurrentTargets { get; private set; }
    public int CurrentTeam { get { return Stats[CurrentUnitIndex].Coordinates.Team; } }
    public int RootPlayer { get; private set; } = -1;

    public BoardState(List<Unit> units, UnitsOrder order)
    {
        foreach (var unit in units)
        {
            UnitStats stats = unit.Stats;
            Stats[stats.Coordinates.Index] = stats;
        }
        Order = order;
    }
    public BoardState(BoardState other)
    {
        foreach (UnitStats stats in other.Stats)
        {
            if (stats != null)
            {
                Stats[stats.Coordinates.Index] = new(stats);
            }
        }
        Order = new UnitsOrder(other.Order);
        CurrentUnitIndex = other.CurrentUnitIndex;
        CurrentTargets = other.CurrentTargets;
        if(other.RootPlayer == -1)
        {
            RootPlayer = CurrentTeam;
        }
        else
        {
            RootPlayer = other.RootPlayer;
        }
    }
    public List<int> GetTargets()
    {
        CurrentUnitIndex = Order.GetNextUnit(Stats);
        CurrentTargets = ChooseViableTargets();
        return CurrentTargets;
    }
    public List<int> ChooseViableTargets()
    {
        List<int> viableTargets = new();
        UnitStats currentUnit = Stats[CurrentUnitIndex];
        Coordinates unitCoordinates = currentUnit.Coordinates;
        if (currentUnit.Range == Range.Meele)
        {
            if (unitCoordinates.Column == 1)
            {
                if (!IsColumnEmpty(0, unitCoordinates.Team))
                {
                    return viableTargets;
                }
            }
            int enemyColumn = 0;
            if (IsColumnEmpty(0, 1 - unitCoordinates.Team))
            {
                enemyColumn = 1;
            }
            foreach (int rowIndex in unitCoordinates.GetEnemyNeighbours())
            {
                UnitStats testedUnit = Stats[6 - 6 * unitCoordinates.Team + 3 * enemyColumn + rowIndex];
                if (testedUnit != null && !testedUnit.IsDead)
                {
                    viableTargets.Add(testedUnit.Coordinates.Index);
                }
            }
            if (viableTargets.Count == 0)
            {
                UnitStats unitToCheck = Stats[6 - 6 * unitCoordinates.Team + 3 * enemyColumn + 2 - unitCoordinates.Row];
                if (unitToCheck != null && !unitToCheck.IsDead)
                {
                    viableTargets.Add(unitToCheck.Coordinates.Index);
                }
            }
        }
        else
        {
            foreach (int index in Stats.GetTeamIndices(1 - unitCoordinates.Team))
            {
                viableTargets.Add(index);
            }
        }
        return viableTargets;
    }
    public bool IsColumnEmpty(int column, int team)
    {
        for (int i = 6 * team + 3 * column; i < 6 * team + 3 * column + 3; i++)
        {

            if (Stats[i] != null && !Stats[i].IsDead)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsGameWon()
    {
        int currentTeam = Stats[CurrentUnitIndex].Coordinates.Team;
        List<int> indices = Stats.GetTeamIndices(1 - currentTeam);
        return indices.Count == 0;
    }

    public List<BoardState> GetPossibleStates()
    {
        List<BoardState> states = new();
        if (CurrentTargets.Count > 0)
        {
            if (Stats[CurrentUnitIndex].Range == Range.All)
            {
                states.Add(GenerateBoardState(CombatActionType.Attack));
            }
            else
            {
                foreach (int index in CurrentTargets)
                {
                    states.Add(GenerateBoardState(CombatActionType.Attack, index));
                }
            }
        }
        if (states.Count == 0 && IsGameWon())
        {
            return states;
        }

        if (CurrentTeam == RootPlayer)
        {
            foreach (int index in Stats.GetTeamIndices(CurrentTeam))
            {
                if (!Stats[index].IsDefending && index != CurrentUnitIndex)
                {
                    states.Add(GenerateBoardState(CombatActionType.Defend));
                    if (!Stats[CurrentUnitIndex].IsWaiting && Order.Order.Count != 0)
                    {
                        states.Add(GenerateBoardState(CombatActionType.Wait));
                    }
                    break;
                }
            }
        }
        if (states.Count == 0)
        {
            states.Add(GenerateBoardState(CombatActionType.Defend));
        }
        return states;
    }

    public BoardState GenerateBoardState(CombatActionType action, int targetIndex = -1)
    {
        BoardState state = new(this);
        UnitStats currentUnit = state.Stats[CurrentUnitIndex];
        currentUnit.IsDefending = false;
        currentUnit.IsWaiting = false;
        switch (action)
        {
            case CombatActionType.Attack:
                if (currentUnit.Range == Range.All)
                {
                    foreach (int index in state.CurrentTargets)
                    {
                        UnitStats target = state.Stats[index];
                        int damage = currentUnit.Damage;
                        DealDamage(target, damage);
                    }
                }
                else
                {
                    UnitStats target = state.Stats[targetIndex];
                    int damage = currentUnit.Damage;
                    DealDamage(target, damage);
                }
                break;
            case CombatActionType.Defend:
                currentUnit.IsDefending = true;
                break;
            case CombatActionType.Wait:
                currentUnit.IsWaiting = true;
                state.Order.AddToWaitingList(CurrentUnitIndex);
                break;
            default:
                break;
        }
        state.GetTargets();
        return state;
    }

    private void DealDamage(UnitStats target, int damage)
    {
        if (target.IsDefending)
        {
            damage /= 2;
        }
        target.CurrentHealth -= Mathf.Clamp(damage, 0, target.CurrentHealth);
    }

    public BoardState GetRandomState()
    {
        List<BoardState> possibleStates = GetPossibleStates();
        if (possibleStates.Count == 0)
        {
            return null;
        }
        return possibleStates.GetRandomValue();
    }
}
