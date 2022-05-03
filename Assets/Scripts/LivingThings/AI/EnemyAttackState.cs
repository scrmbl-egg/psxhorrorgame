using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _currentTime;
    private float _delayBeforeAttack;
    private bool _hasAttacked;
    private float _delayAfterAttack;

    public override void EnterState(EnemyThing ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(EnemyThing ctx)
    {
        if (!_hasAttacked) CountDelayBeforeAttack(ctx);
        else CountDelayAfterAttack(ctx);
    }

    #region Private methods
    
    private void SetupState(EnemyThing ctx)
    {
        ctx.Agent.isStopped = true;

        _currentTime = 0;
        _hasAttacked = false;
        _delayBeforeAttack = ctx.RandomStateDelayBeforeAttack;
        _delayAfterAttack = ctx.RandomStateDelayAfterAttack;
    }

    private void CountDelayBeforeAttack(EnemyThing ctx)
    {
        _currentTime += Time.deltaTime;

        bool timerIsDone = _currentTime >= _delayBeforeAttack;
        if (timerIsDone)
        {
            Attack(ctx);
            _hasAttacked = true;
        }
    }

    private void Attack(EnemyThing ctx)
    {
        Debug.Log("attack");
        
        Vector3 origin = ctx.transform.position;
        float radius = 3;

        Collider[] colliders = Physics.OverlapSphere(origin, radius, ctx.AttackLayers);
        foreach(Collider collider in colliders)
        {
            bool colliderIsPlayer = collider.transform.CompareTag("Player");
            if (colliderIsPlayer)
            {
                var target = collider.GetComponentInParent<LivingThing>();

                target.Health -= ctx.Damage;
            }
        }
    }

    private void CountDelayAfterAttack(EnemyThing ctx)
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
