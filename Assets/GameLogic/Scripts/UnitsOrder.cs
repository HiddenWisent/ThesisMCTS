using System;
using System.Collections.Generic;

[Serializable]
public class UnitsOrder
{
    private int _lastUnitIndex = -1;
    private List<int> _unitsIndices = new();
    private List<int> _order = new();
    private List<int> _waitingList = new();

    public List<int> Order { get { return _order; } }
    public UnitsOrder(List<Unit> passedUnits)
    {
        passedUnits.Sort();
        foreach (var unit in passedUnits)
        {
            _unitsIndices.Add(unit.Stats.Coordinates.Index);
        }
    }

    public UnitsOrder(UnitsOrder other)
    {
        _lastUnitIndex = other._lastUnitIndex;
        _order = new(other._order);
        _unitsIndices = new(other._unitsIndices);
        _waitingList = new(other._waitingList);
    }
    internal int GetNextUnit(UnitStats[] stats)
    {
        if(_order.Count == 0)
        {
            if (_waitingList.Count > 0)
            {
                _order = _waitingList;
                _waitingList = new();
            }
            else
            {
                PopulateQueue();
                if (_lastUnitIndex == -1)
                {
                    return ReturnIndex(_order[0], stats);
                }
            }
        }
        int firstUnitSpeed = stats[_order[0]].Speed;
        foreach (int index in _order)
        {
            if (stats[index].Speed == firstUnitSpeed)
            {
                if (stats[index].Coordinates.Team != stats[_lastUnitIndex].Coordinates.Team)
                {
                    return ReturnIndex(index, stats);
                }
            }
            else
            {
                break;
            }
        }
        return ReturnIndex(_order[0], stats);
    }

    private int ReturnIndex(int index, UnitStats[] stats)
    {
        if (stats[index].IsDead)
        {
            _order.Remove(index);
            return GetNextUnit(stats);
        }
        _lastUnitIndex = index;
        _order.Remove(index);
        return _lastUnitIndex;
    }

    private void PopulateQueue()
    {
        foreach (int index in _unitsIndices) 
        {
            _order.Add(index);
        }
    }

    internal void AddToWaitingList(int index)
    {
        _waitingList.Add(index);
    }
}
