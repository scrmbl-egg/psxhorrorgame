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
        //death effects
    }

    public override void DamageEffect()
    {
        Bleed();
    }

    public override void HealingEffect()
    {
        base.HealingEffect();

        //healing effects
    }
}
