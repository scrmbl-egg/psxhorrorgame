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
    public float MovementSpeed => movementSpeed;
    public int Damage => Random.Range(minDamage, maxDamage + 1);

    #region Public methods

    public virtual void Attack()
    {
        Debug.Log($"{ThingName}: I attack with {Damage} damage");
    }

    public override void DeathEffect()
    {
        base.DeathEffect();

        //TODO: trigger ragdoll physics
    }

    public override void DamageEffect()
    {
        Debug.Log($"{ThingName} enemy has been hit!");
        //TODO: do damage sound
    }

    #endregion
}