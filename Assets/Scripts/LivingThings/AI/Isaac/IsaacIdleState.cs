using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacIdleState : IsaacState
{
    private float _alertRange;

    public override void EnterState(IsaacEnemy ctx)
    {
        SetupState(ctx);
        Debug.Log($"{ctx.ThingName} is now in idle state!");
    }

    public override void UpdateState(IsaacEnemy ctx)
    {
        AlertDetection(ctx);
    }

    #region Private methods

    private void SetupState(IsaacEnemy ctx)
    {
        ctx.Agent.isStopped = true;

        _alertRange = ctx.AlertRange;
    }

    private void AlertDetection(IsaacEnemy ctx)
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