using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OnValueChangedText : MonoBehaviour
{
    [SerializeField]
    private bool _wholeNumbers;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnSliderValueChange(float value)
    {
        if (_wholeNumbers)
        {
            _text.text = value.ToString("0");
        }
        else
        {
            _text.text = value.ToString("0.00");
        }
    }
}
