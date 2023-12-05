using UnityEngine;
using TMPro;

public class AIChooser : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _dropdown;
    [SerializeField]
    private CanvasGroup _valueChooser;
    private AI _player;

    private void Awake()
    {
        ToggleIterationSelection(0);
    }
    public void CreateAI(GameObject player)
    {
        Destroy(_player);
        switch (_dropdown.value)
        {
            case 1:
                _player = player.AddComponent<RandomAI>();
                break;
            case 2:
                _player = player.AddComponent<MCTSAI>();
                break;
            default:
                break;
        }
    }

    public void ToggleIterationSelection(int value)
    {
        switch (value) 
        {
            case 2:
                _valueChooser.transform.SetParent(transform);
                _valueChooser.alpha = 1;
                break;
            default:
                _valueChooser.transform.SetParent(_dropdown.transform);
                _valueChooser.alpha = 0;
                break;
        }
    }

    public void SetIterations(float value)
    {
        if(_player is MCTSAI)
        {
            _player.GetComponent<MCTSAI>().SetIterations((int)value);
        }
    }
}
