using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyThing ctx)
    {
        Debug.Log($"{ctx} has entered the attack state");
    }

    public override void UpdateState(EnemyThing ctx)
    {
    }
}
