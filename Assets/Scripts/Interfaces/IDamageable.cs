using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for elements that can be damaged, healed, and killed.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Subtracts a given value from the owner class' health.
    /// </summary>
    /// <param name="dmgAmount">Amount of damage.</param>
    public void Damage(int dmgAmount);

    /// <summary>
    /// Adds a given value to the owner class' health.
    /// </summary>
    /// <param name="hpAmount">Amount of health points.</param>
    public void Heal(int hpAmount);

    /// <summary>
    /// Represents the moment the class is out of health.
    /// </summary>
    /// <remarks>
    /// It is recommended to call all ragdoll physics here when the class is a live entity.
    /// </remarks>
    void Death();
}
