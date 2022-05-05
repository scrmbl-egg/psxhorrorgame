using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacEnemy : EnemyThing
{
    private IsaacState _currentState;
    public IsaacIdleState IdleState { get; } = new IsaacIdleState();
    public IsaacChaseState ChaseState { get; } = new IsaacChaseState();
    public IsaacAttackState AttackState { get; } = new IsaacAttackState();

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();

        SetState(IdleState);
    }

    private void Update()
    {
        _currentState?.UpdateState(this);
    }

    #endregion

    #region Public methods

    #region Health and death

    public override void DeathEffect()
    {
        //HACK: Trigger ragdoll physics when enemy is fully animated
        _currentState = null;

        Agent.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public override void DamageEffect()
    {
        bool isIdle = _currentState == IdleState;
        if (isIdle)
        {
            SetTarget(PlayerTarget);
            SetState(ChaseState);
        }
    }

    #endregion

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