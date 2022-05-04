using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacIdleState : IsaacState
{


    public override void EnterState(IsaacEnemy ctx)
    {
        SetupState(ctx);
    }

    public override void UpdateState(IsaacEnemy ctx)
    {
        
    }

    #region Private methods

    private void SetupState(IsaacEnemy ctx)
    {
        ctx.Agent.isStopped = true;
        Debug.Log($"{ctx.ThingName} is now in idle state!");
    }

    #endregion
}