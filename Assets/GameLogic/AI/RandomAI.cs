using System;
using System.Collections.Generic;

public class RandomAI : AI
{
    internal override void EvaluateTargets(List<Unit> viableTargets, Range range, CombatSystem board, float time)
    {
        int targetsCount = viableTargets.Count;
        if(targetsCount == 0)
        {
            OnAIAction(CombatActionType.Defend, null);
            return;
        }
        if(range == Range.All)
        {
            OnAIAction(CombatActionType.Attack, viableTargets[0]);
            return;
        }
        int index = UnityEngine.Random.Range(0, targetsCount);
        OnAIAction(CombatActionType.Attack, viableTargets[index]);
    }
}
