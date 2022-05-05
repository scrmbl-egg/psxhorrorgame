using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacChaseState : IsaacState
{
    private float _attentionSpan;
    private float _currentAttentionSpan;
    private float _alertRange;
    private float _attackDistanceThreshold;

    public override void EnterState(IsaacEnemy ctx)
    {
        SetupState(ctx);
        Debug.Log("isaac chase");
    }

    public override void UpdateState(IsaacEnemy ctx)
    {
        ManageAttentionSpan(ctx);
        ChaseTarget(ctx);
    }

    #region Private methods

    private void SetupState(IsaacEnemy ctx)
    {
        _attentionSpan = ctx.AttentionSpan;
        _alertRange = ctx.AlertRange;
        _attackDistanceThreshold = ctx.AttackDistanceThreshold;

        _currentAttentionSpan = _attentionSpan;

        ctx.Agent.isStopped = false;
    }

    private void ManageAttentionSpan(IsaacEnemy ctx)
    {
        //slowly decrease attention towards current target if it's not in range
        _currentAttentionSpan -= Time.deltaTime;

        float distanceFromTarget = Vector3.Distance(ctx.transform.position, EnemyThing.PlayerTarget.position);
        bool targetIsInAlertRange = distanceFromTarget <= _alertRange;
        bool thereIsAttentionSpanLeft = _currentAttentionSpan > 0;

        if (targetIsInAlertRange)
        {
            _currentAttentionSpan = _attentionSpan;
        }

        if (thereIsAttentionSpanLeft) return;
        //else...

        ctx.SetState(ctx.IdleState);
    }

    private void ChaseTarget(IsaacEnemy ctx)
    {
        ctx.Agent.SetDestination(ctx.Target.position);

        float threshold = _attackDistanceThreshold + ctx.Agent.stoppingDistance;
        float distanceFromTarget = Vector3.Distance(ctx.transform.position, ctx.Target.position);
        bool targetIsInRange = distanceFromTarget <= threshold;
        bool targetIsPlayer = ctx.Target == EnemyThing.PlayerTarget;

        if (targetIsInRange && targetIsPlayer) ctx.SetState(ctx.AttackState);
        else if (targetIsInRange && !targetIsPlayer) ctx.SetState(ctx.IdleState);
    }

    #endregion
}