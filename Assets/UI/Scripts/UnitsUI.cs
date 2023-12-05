using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsUI : MonoBehaviour
{
    public UnitAnchor[] Anchors = new UnitAnchor[12];

    public List<Unit> GetUnits()
    {
        List<Unit> units = new();
        foreach (UnitAnchor anchors in Anchors)
        {
            if (anchors.Unit != null) 
            {
                units.Add(anchors.Unit);
            }
        }
        return units;
    }
}
