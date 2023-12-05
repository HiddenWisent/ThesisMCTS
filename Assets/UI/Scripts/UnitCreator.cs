using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCreator : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private TMP_InputField _damageInputField;
    [SerializeField]
    private TMP_InputField _healthInputField;
    [SerializeField]
    private TMP_InputField _speedInputField;
    [SerializeField]
    private TMP_Dropdown _rangeDropdown;
    [SerializeField]
    private Image _image;
    [Header("")]
    [SerializeField]
    private Unit _unitTemplate;
    [SerializeField]
    private CombatSystem _combatSystem;
    [SerializeField]
    private UnitsUI _anchors;

    public void CreateUnit()
    {
        if (_damageInputField.text == "" || _healthInputField.text == "" || _speedInputField.text == "")
        {
            return;
        }
        Coordinates coordinates = GetFreeCoordinates(_anchors.GetUnits());
        if (coordinates != null)
        {
            int damage = Convert.ToInt32(_damageInputField.text);
            int health = Convert.ToInt32(_healthInputField.text);
            int speed = Convert.ToInt32(_speedInputField.text);
            Range range = (Range)_rangeDropdown.value;
            Unit newUnit = Instantiate(_unitTemplate, _combatSystem.transform);
            newUnit.SetStats(coordinates, damage, health, speed, range);
            newUnit.Sprite = _image.sprite;
            _anchors.Anchors[coordinates.Index].AddUnit(newUnit, newUnit.Sprite);
        } 
    }

    public Coordinates GetFreeCoordinates(List<Unit> units)
    {
        bool[] indices = new bool[12];
        foreach (Unit unit in units)
        {
            indices[unit.Stats.Coordinates.Index] = true;
        }
        for (int i = 0; i < indices.Length; i++)
        {
            if (!indices[i])
            {
                return new Coordinates(i);
            }
        }
        return null;
    }
}
