using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
    typeof(PlayerMovement),
    typeof(PlayerLook),
    typeof(PlayerInteraction)
    )]
public class PlayerThing : LivingThing
{

    public override void DeathEffect()
    {
        base.DeathEffect();

        //death effects
    }

    public override void DamageEffect()
    {
        base.DamageEffect();

        //damage effects
    }

    public override void HealingEffect()
    {
        base.HealingEffect();

        //healing effects
    }

}
