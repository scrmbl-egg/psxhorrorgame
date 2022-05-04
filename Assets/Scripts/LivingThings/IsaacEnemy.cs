using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacEnemy : EnemyThing
{
    private IsaacState _currentState;
    private IsaacIdleState _idleState = new IsaacIdleState();
    private IsaacChaseState _chaseState = new IsaacChaseState();
    private IsaacAttackState _attackState = new IsaacAttackState();

    public IsaacIdleState IdleState => _idleState;
    public IsaacChaseState ChaseState => _chaseState;
    public IsaacAttackState AttackState => _attackState;

    #region MonoBehaviour

    private void Awake()
    {
        SetState(IdleState);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    #endregion

    #region Public methods

    public void SetState(IsaacState state)
    {
        _currentState = state;
        state.EnterState(this);
    }

    public override void Attack()
    {
        base.Attack();
    }

    #endregion
    #region Private methods

    #endregion
}