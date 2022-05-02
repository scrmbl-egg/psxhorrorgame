using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private float _attentionSpan;
    private float _currentAttentionSpan;
    
    public override void EnterState(EnemyThing ctx)
    {
        SetupAttentionSpan(ctx.AttentionSpan);
    }

    public override void UpdateState(EnemyThing ctx)
    {
        DecreaseAttentionSpan();
        ChaseTarget(ctx.Agent, ctx.Target);
    }

    #region Private methods
    private void SetupAttentionSpan(float attentionSpan)
    {
        _attentionSpan = attentionSpan;
        _currentAttentionSpan = 0;
    }

    private void DecreaseAttentionSpan()
    {
        //TODO: DECREASE ATTENTION SPAN
    }

    private void ChaseTarget(NavMeshAgent agent, Transform target)
    {
        agent.SetDestination(target.position);
    }

    #endregion
}