using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MCTSAI : AI
{

    public int Iterations { get; private set; } = 1500;
    private List<Unit> _viableTargets;
    private Range _range;
    [SerializeField]
    private MCTree _tree;
    internal override void EvaluateTargets(List<Unit> viableTargets, Range range, CombatSystem combatSystem, float time)
    {
        _viableTargets = viableTargets;
        _range = range;
        BoardState board = new(combatSystem.Board);
        _tree = new(board);
        float constant = math.sqrt(2);
        StartCoroutine(SearchTree(constant, time));
    }
    public IEnumerator SearchTree(float selectionConstant, float maxTime)
    {
        MCNode root = _tree.Root;
        float timePassed = 0f;
        while (timePassed < maxTime && root.State.Visits < Iterations && !root.IsFullyExplored)
        {
            for (int i = 0; i < 100; i++)
            {
                root.SelectNode(selectionConstant);
                if (root.IsFullyExplored)
                {
                    break;
                }
            }
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        int actionIndex = _tree.GetBestMove();
        GiveEvaluation(actionIndex);
    }

    private void GiveEvaluation(int actionIndex)
    {
        int maxActionCount = _viableTargets.Count + 1;
        CombatActionType action;
        Unit target = null;
        if (_range == Range.All)
        {
            maxActionCount = 2;
        }
        switch (maxActionCount - actionIndex)
        {
            case 0:
                action = CombatActionType.Wait;
                break;
            case 1:
                action = CombatActionType.Defend;
                break;
            default:
                action = CombatActionType.Attack;
                target = _viableTargets[actionIndex];
                break;
        }
        OnAIAction(action, target);
    }
    public void SetIterations(int value)
    {
        Iterations = value;
    }
}
