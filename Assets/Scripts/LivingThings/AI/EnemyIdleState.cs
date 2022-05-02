using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyThing ctx)
    {
        Debug.Log($"{ctx} has entered the idle state");
    }

    public override void UpdateState(EnemyThing ctx)
    {
    }
}
