using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Wait,
    Act
}
public class CombatSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerTemplate;
    [SerializeField]
    private UnitsUI _anchors;
    [SerializeField]
    private List<GameObject> _playerObjects;
    private float _minSpeed = 0.75f;
    private float _maxSpeed = 2f;
    private float _waitingTime;
    public BoardState Board { get; private set; }
    public UnitsOrder Order { get; private set; }
    public List<Unit> AllUnits { get; private set; } = new();
    public Unit[] UnitsArray { get; private set; } = new Unit[12];
    public Unit CurrentUnit { get { return UnitsArray[Board.CurrentUnitIndex]; } }
    private Unit _target;
    public AI[] Players { get; private set; } = new AI[2];
    private GameState _gameState;

    public List<Unit> ViableTargets { get; private set; }

    private void Start()
    {
        UIEvents.instance.OnUnitClicked += HumanAttack;
    }
    public void CombatStart()
    {
        AllUnits = _anchors.GetUnits();
        if(AllUnits.Count == 0 )
        {
            return;
        }
        foreach( Unit unit in AllUnits )
        {
            int index = unit.Stats.Coordinates.Index;
            UnitsArray[index] = unit;
        }
        Order = new(AllUnits);
        Board = new(AllUnits, Order);
        Debug.Log("Combat Start!");
        _gameState = GameState.Wait;
        InitiatePlayers();
        UIEvents.instance.CombatStart();
        NewTurn();
    }

    public void CombatEnd()
    {
        foreach (Unit unit in AllUnits)
        {
            unit.Events.Reset();
        }
        AllUnits = null;
        Board = null;
        Order = null;
        DeactivatePlayers();
        StopAllCoroutines();
        UIEvents.instance.CombatEnd();
    }
    private void NewTurn()
    {
        StopAllCoroutines();
        ViableTargets = UnitsArray.GetUnitsByIndices(Board.GetTargets());
        CurrentUnit.Events.TurnStart();
        int teamIndex = CurrentUnit.Stats.Coordinates.Team;
        if (Players[teamIndex] != null)
        {
            StartCoroutine(AITurn());
        }
        else
        {
            foreach (Unit target in ViableTargets)
            {
                target.Events.IsTargeted();
            }
            _gameState = GameState.Act;
        }
    }

    public void HumanAttack(Unit unitHit, int button)
    {
        if (_gameState == GameState.Act && button == -1)
        {
            if (ViableTargets.Contains(unitHit))
            {
                _target = unitHit;
                _gameState = GameState.Wait;
                UnitMove(CombatActionType.Attack);
            }
        }
    }

    public void HumanMoveButton(int index)
    {
        if( _gameState == GameState.Act)
        {
            CombatActionType actionType;
            switch (index)
            {
                case 0:
                    actionType = CombatActionType.Defend;
                    break;
                case 1:
                    if (CurrentUnit.Stats.IsWaiting)
                    {
                        return;
                    }
                    actionType = CombatActionType.Wait;
                    break;
                default:
                    return;
            }
            _gameState = GameState.Wait;
            UnitMove(actionType);
        }
    }

    private void UnitMove(CombatActionType action)
    {
        switch (action)
        {
            case CombatActionType.Defend:
                CurrentUnit.Events.Defend();
                break;
            case CombatActionType.Wait:
                CurrentUnit.Events.Wait();
                Order.AddToWaitingList(CurrentUnit.Stats.Coordinates.Index);
                break;
            case CombatActionType.Attack:
                if (CurrentUnit.Stats.Range == Range.All)
                {
                    foreach (Unit target in ViableTargets)
                    {
                        Attack(CurrentUnit, target);
                    }
                }
                else
                {
                    Attack(CurrentUnit, _target);
                }
                break;
            default:
                break;
        }
        EndTurn();
    }

    private IEnumerator AITurn()
    {
        _waitingTime = Time.time + _minSpeed;
        AI currentPlayer = Players[CurrentUnit.Stats.Coordinates.Team];
        currentPlayer.EvaluateTargets(ViableTargets, CurrentUnit.Stats.Range, this, _maxSpeed);
        yield return null;
    }

    private void EndTurn()
    {
        foreach (Unit unit in ViableTargets)
        {
            unit.Events.TurnEnd();
        }
        if (Board.IsGameWon())
        {
            Debug.Log("Team " +  CurrentUnit.Stats.Coordinates.Team + " won!");
            return;
        }
        NewTurn();
    }

    public void Attack(Unit attacker, Unit target)
    {
        attacker.Events.Attack(target);
    }
    
    public void SetMinSpeed(float speed)
    {
        _minSpeed = speed;
    }
    public void SetMaxSpeed(float speed)
    {
        _maxSpeed = speed;
    }
    private void ReactToAIAction(object sender, AIEventArgs e)
    {
        _waitingTime -= Time.time;
        _waitingTime = Math.Clamp(_waitingTime, 0f, _minSpeed);
        _target = e.Target;
        StartCoroutine(WaitForAI(e.ActionType));
    }

    private IEnumerator WaitForAI(CombatActionType action)
    {
        yield return new WaitForSeconds(_waitingTime);
        UnitMove(action);
    }
    private void OnDestroy()
    {
        UIEvents.instance.OnUnitClicked -= HumanAttack;
        if (Players[0] != null)
        {
            Players[0].AIAction -= ReactToAIAction;
        }
        if (Players[1] != null)
        {
            Players[1].AIAction -= ReactToAIAction;
        }
    }

    private void InitiatePlayers()
    {
        Players[0] = _playerObjects[0].GetComponent<AI>();
        if (Players[0] != null)
        {
            Players[0].AIAction += ReactToAIAction;
        }
        Players[1] = _playerObjects[1].GetComponent<AI>(); 
        if (Players[1] != null)
        {
            Players[1].AIAction += ReactToAIAction;
        }
    }

    private void DeactivatePlayers()
    {
        if (Players[0] != null)
        {
            Players[0].AIAction -= ReactToAIAction;
        }
        if (Players[1] != null)
        {
            Players[1].AIAction -= ReactToAIAction;
        }
    }
}
