using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacAttackState : IsaacState
{
    private float _currentTime;
    private float _delayBeforeAttack;
    private bool _hasAttacked;
    private float _delayAfterAttack;

    public override void EnterState(IsaacEnemy ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(IsaacEnemy ctx)
    {
        if (_hasAttacked) CountDelayAfterAttack(ctx);
        else CountDelayBeforeAttack(ctx); ;
    }

    #region Private methods

    private void SetupState(IsaacEnemy ctx)
    {
        ctx.Agent.isStopped = true;

        _currentTime = 0;
        _hasAttacked = false;
        _delayBeforeAttack = ctx.RandomStateDelayBeforeAttack;
        _delayAfterAttack = ctx.RandomStateDelayAfterAttack;
    }

    private void CountDelayBeforeAttack(IsaacEnemy ctx)
    {
        _currentTime += Time.deltaTime;

        bool timerIsDone = _currentTime >= _delayBeforeAttack;
        if (timerIsDone)
        {
            ctx.Attack();
            _hasAttacked = true;
        }
    }

    private void CountDelayAfterAttack(IsaacEnemy ctx)
    {
        _currentTime += Time.deltaTime;

        bool timerIsDone = _currentTime >= _delayAfterAttack;
        if (timerIsDone)
        {
            ctx.SetState(ctx.ChaseState);
        }
    }

    #endregion
}
