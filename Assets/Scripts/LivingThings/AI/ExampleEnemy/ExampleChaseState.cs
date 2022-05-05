using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleChaseState : ExampleEnemyState
{
    private float _attentionSpan;
    private float _currentAttentionSpan;
    private float _alertRange;
    private float _attackDistanceThreshold;
    
    public override void EnterState(ExampleEnemy ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(ExampleEnemy ctx)
    {
        ManageAttentionSpan(ctx);
        ChaseTarget(ctx);
    }

    #region Private methods

    private void SetupState(ExampleEnemy ctx)
    {
        _attentionSpan = ctx.AttentionSpan;
        _alertRange = ctx.AlertRange;
        _attackDistanceThreshold = ctx.AttackDistanceThreshold;

        _currentAttentionSpan = _attentionSpan;

        ctx.Agent.isStopped = false;
    }

    private void ManageAttentionSpan(ExampleEnemy ctx)
    {
        //slowly decrease attention towards current target if it's not in range
        _currentAttentionSpan -= Time.deltaTime;

        float distanceFromTarget = Vector3.Distance(ctx.transform.position, EnemyThing.PlayerTarget.position);
        bool targetIsInRange = distanceFromTarget <= _alertRange;
        bool thereIsAttentionSpanLeft = _currentAttentionSpan > 0;

        if (targetIsInRange)
        {
            //reset current attention span
            _currentAttentionSpan = _attentionSpan;
            return;
        }

        if (thereIsAttentionSpanLeft) return;
        //else...

        ctx.SetState(ctx.IdleState);
    }

    private void ChaseTarget(ExampleEnemy ctx)
    {
        ctx.Agent.SetDestination(ctx.Target.position);

        float threshHold = ctx.Agent.stoppingDistance + _attackDistanceThreshold;
        float distanceFromTarget = Vector3.Distance(ctx.transform.position, ctx.Target.position);
        bool targetIsInRange = distanceFromTarget <= threshHold;
        bool targetIsPlayer = ctx.Target == EnemyThing.PlayerTarget;

        if (targetIsInRange && targetIsPlayer) ctx.SetState(ctx.AttackState);
        else if (targetIsInRange && !targetIsPlayer) ctx.SetState(ctx.IdleState);
    }

    #endregion
}