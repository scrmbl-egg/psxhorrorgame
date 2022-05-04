using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAttackState : ExampleEnemyState
{
    private float _currentTime;
    private float _delayBeforeAttack;
    private bool _hasAttacked;
    private float _delayAfterAttack;

    public override void EnterState(ExampleEnemy ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(ExampleEnemy ctx)
    {
        if (_hasAttacked) CountDelayAfterAttack(ctx);
        else CountDelayBeforeAttack(ctx); ;
    }

    #region Private methods
    
    private void SetupState(ExampleEnemy ctx)
    {
        ctx.Agent.isStopped = true;

        _currentTime = 0;
        _hasAttacked = false;
        _delayBeforeAttack = ctx.RandomStateDelayBeforeAttack;
        _delayAfterAttack = ctx.RandomStateDelayAfterAttack;
    }

    private void CountDelayBeforeAttack(ExampleEnemy ctx)
    {
        _currentTime += Time.deltaTime;

        bool timerIsDone = _currentTime >= _delayBeforeAttack;
        if (timerIsDone)
        {
            ctx.Attack();
            _hasAttacked = true;
        }
    }

    private void CountDelayAfterAttack(ExampleEnemy ctx)
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