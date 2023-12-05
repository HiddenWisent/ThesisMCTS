using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _damageText;
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private TextMeshProUGUI _speedText;
    [SerializeField]
    private TextMeshProUGUI _rangeText;
    [SerializeField]
    private Image _image;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        UIEvents.instance.OnUnitPressed += OpenPanel;
        UIEvents.instance.OnUnitReleased += ClosePanel;
    }
    public void OpenPanel(Unit unit, int button)
    {
        if (button == -2) 
        {
            UnitStats stats = unit.Stats;
            _damageText.text = stats.Damage + "";
            _healthText.text = stats.CurrentHealth + "/" + stats.MaxHealth;
            _speedText.text = stats.Speed + "";
            _rangeText.text = stats.Range + "";
            _image.sprite = unit.Sprite;
            _canvasGroup.alpha = 1f;
        }

    }

    public void ClosePanel(int button)
    {
        if (button == -2)
        {
            _canvasGroup.alpha = 0f;
        }
    }

    private void OnDestroy()
    {
        UIEvents.instance.OnUnitPressed -= OpenPanel;
        UIEvents.instance.OnUnitReleased -= ClosePanel;
    }
}
