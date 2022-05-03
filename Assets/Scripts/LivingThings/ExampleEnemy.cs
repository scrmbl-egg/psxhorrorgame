using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEnemy : EnemyThing
{
    private ExampleEnemyState _currentState;
    private ExampleIdleState _idleState = new ExampleIdleState();
    private ExampleChaseState _chaseState = new ExampleChaseState();
    private ExampleAttackState _attackState = new ExampleAttackState();

    public ExampleIdleState IdleState => _idleState;
    public ExampleChaseState ChaseState => _chaseState;
    public ExampleAttackState AttackState => _attackState;

    #region MonoBehaviour

    private void Awake()
    {
        Player = FindObjectOfType<PlayerThing>().transform;
        SetState(IdleState);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    #endregion

    #region Public methods

    public void SetState(ExampleEnemyState state)
    {
        _currentState = state;
        state.EnterState(this);
    }

    public override void Attack()
    {
        base.Attack();
    }

    #endregion
}