using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleIdleState : ExampleEnemyState
{
    private float _alertRange;

    public override void EnterState(ExampleEnemy ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(ExampleEnemy ctx)
    {
        AlertDetection(ctx);
    }

    #region Private methods

    private void SetupState(ExampleEnemy ctx)
    {
        _alertRange = ctx.AlertRange;
        ctx.Agent.isStopped = true;
    }

    private void AlertDetection(ExampleEnemy ctx)
    {
        float distanceFromPlayer = Vector3.Distance(ctx.transform.position, EnemyThing.PlayerTarget.position);
        bool playerIsNotInRange = distanceFromPlayer > _alertRange;

        if (playerIsNotInRange) return;
        //else...

        ctx.SetTarget(EnemyThing.PlayerTarget);
        ctx.SetState(ctx.ChaseState);
    }

    #endregion
}