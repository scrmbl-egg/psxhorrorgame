using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private float _attentionSpan;
    private float _currentAttentionSpan;
    private float _alertRange;
    private float _attackDistanceThreshold;
    private float _currentTimeBeforeAttack;
    private float _timeBeforeAttack;
    
    public override void EnterState(EnemyThing ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(EnemyThing ctx)
    {
        ManageAttentionSpan(ctx);
        ChaseTarget(ctx);
    }

    #region Private methods

    private void SetupState(EnemyThing ctx)
    {
        _attentionSpan = ctx.AttentionSpan;
        _alertRange = ctx.AlertRange;
        _attackDistanceThreshold = ctx.AttackDistanceThreshold;
        _timeBeforeAttack = ctx.RandomStateDelayBeforeAttack;

        _currentAttentionSpan = 0;
        _currentTimeBeforeAttack = 0;

        ctx.Agent.isStopped = false;
    }

    private void ManageAttentionSpan(EnemyThing ctx)
    {
        _currentAttentionSpan += Time.deltaTime;

        float distanceToTarget = Vector3.Distance(ctx.transform.position, ctx.Player.position);
        bool targetIsInRange = distanceToTarget <= _alertRange;
        bool thereIsAttentionSpanLeft = _currentAttentionSpan < _attentionSpan;

        if (targetIsInRange)
        {
            _currentAttentionSpan = 0;
            return;
        }

        if (thereIsAttentionSpanLeft) return;
        //else...

        _currentAttentionSpan = 0;
        ctx.SetState(ctx.IdleState);
    }

    private void ChaseTarget(EnemyThing ctx)
    {
        ctx.Agent.SetDestination(ctx.Target.position);

        float threshHold = ctx.Agent.stoppingDistance + _attackDistanceThreshold;
        float distanceToTarget = Vector3.Distance(ctx.transform.position, ctx.Target.position);
        bool targetIsInRange = distanceToTarget <= threshHold;
        bool targetIsPlayer = ctx.Target == ctx.Player;

        if (targetIsInRange && targetIsPlayer)
        {
            _currentTimeBeforeAttack += Time.deltaTime;

            bool attackTimerIsNotDone = _currentTimeBeforeAttack < _timeBeforeAttack;
            if (attackTimerIsNotDone) return;
            //else...

            _currentTimeBeforeAttack = 0;
            ctx.SetState(ctx.AttackState);
        }
    }

    #endregion
}