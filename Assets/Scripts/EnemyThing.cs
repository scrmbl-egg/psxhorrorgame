using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThing : LivingThing
{
    [Header("Enemy Properties")]
    //
    [SerializeField] private float movementSpeed;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    public float MovementSpeed { get; private set; }
    public int CurrentDamage { get; private set; }

    #region Public methods

    public virtual void Attack()
    {
        int randomDamage = Random.Range(minDamage, maxDamage + 1);

        CurrentDamage = randomDamage;
        Debug.Log($"{ThingName}: I attack.");
    }

    public override void DeathEffect()
    {
        base.DeathEffect();

        //TODO: trigger ragdoll physics
    }

    public override void DamageEffect()
    {
        base.DamageEffect();

        //TODO: spawn blood splats
    }

    #endregion
}