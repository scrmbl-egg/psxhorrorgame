using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyThing ctx)
    {
        Debug.Log($"{ctx} has entered the chase state");
    }

    public override void UpdateState(EnemyThing ctx)
    {
    }
}