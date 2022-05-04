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
        float distanceToPlayer = Vector3.Distance(ctx.transform.position, ctx.Player.position);
        bool playerIsNotInRange = distanceToPlayer > _alertRange;

        if (playerIsNotInRange) return;
        //else...

        ctx.SetTarget(ctx.Player);
        ctx.SetState(ctx.ChaseState);
    }

    #endregion
}