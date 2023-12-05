using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour
{
    internal abstract void EvaluateTargets(List<Unit> viableTarget, Range range, CombatSystem board, float time);

    public event EventHandler<AIEventArgs> AIAction;
    
    protected virtual void OnAIAction(CombatActionType action, Unit target)
    {
        EventHandler<AIEventArgs> handler = AIAction;
        handler?.Invoke(this, new AIEventArgs(action, target));
    }
    public void DestroyComponent()
    {
        Destroy(this);
    }
}

public delegate void AIEventHandler(object sender, AIEventArgs e);

public class AIEventArgs : EventArgs
{
    public CombatActionType ActionType { get; set; }
    public Unit Target { get; set; }
    public AIEventArgs(CombatActionType action, Unit target)
    {
        ActionType = action;
        Target = target;
    }
}