using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEnemy : EnemyThing
{
    private ExampleEnemyState _currentState;

    public ExampleIdleState IdleState { get; } = new ExampleIdleState();
    public ExampleChaseState ChaseState { get; } = new ExampleChaseState();
    public ExampleAttackState AttackState { get; } = new ExampleAttackState();

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();

        SetState( IdleState );
    }

    private void Update()
    {
        _currentState.UpdateState( this );
    }

    #endregion

    #region Public methods

    public override void DamageEffect()
    {
        bool isIdle = _currentState == IdleState;
        if (isIdle)
        {
            SetTarget( PlayerTarget );
            SetState( ChaseState );
        }
    }

    public void SetState( ExampleEnemyState state )
    {
        _currentState = state;
        state.EnterState( this );
    }

    public override void Attack()
    {
        base.Attack();
    }

    #endregion
}