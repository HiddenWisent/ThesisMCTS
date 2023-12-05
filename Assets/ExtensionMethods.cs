using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static List<int> GetTeamIndices(this UnitStats[] stats, int teamIndex)
    {
        List<int> indices = new();
        for (int i = 6 * teamIndex; i < 6 * teamIndex + 6; i++)
        {
            if (stats[i] != null && !stats[i].IsDead)
            {
                indices.Add(i);
            }
        }
        return indices;
    }

    public static UnitStats[] GetStatsArray(this Unit[] units)
    {
        UnitStats[] stats = new UnitStats[units.Length];
        foreach (Unit unit in units)
        {
            if(unit != null)
            {
                stats[unit.Stats.Coordinates.Index] = unit.Stats;
            }
        }
        return stats;
    }

    public static List<UnitStats> GetStatsList(this List<Unit> units) 
    { 
        List<UnitStats> stats = new();
        foreach (Unit unit in units)
        {
            stats.Add(unit.Stats);
        }
        return stats;
    }

    public static List<Unit> GetUnitsByIndices(this Unit[] units, List<int>indices)
    {
        List<Unit> unitsList = new();
        foreach (Unit unit in units)
        {
            if (unit != null && indices.Contains(unit.Stats.Coordinates.Index))
            {
                unitsList.Add(unit);
            }
        }
        return unitsList;
    }

    public static T GetRandomValue<T>(this List<T> list) 
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static List<T> CreateListOfComponent<T>(this List<GameObject> list)
    {
        List<T> returnList = new();
        foreach (GameObject gameObject in list)
        {
            T newItem = gameObject.GetComponent<T>();
            returnList.Add(newItem);
        }
        return returnList;
    }
}
