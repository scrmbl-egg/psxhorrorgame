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
    [Header("Dependencies")]
    //
    [SerializeField] private CamShake camShake;

    private PlayerMovement _movement;
    private PlayerLook _look;
    private PlayerInteraction _interaction;

    public override void DeathEffect()
    {
        //death effects
    }

    public override void DamageEffect()
    {
        Bleed();
        camShake.ShakeCamera();
    }

    public override void HealingEffect()
    {
        base.HealingEffect();

        //healing effects
    }
}
