using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThing : MonoBehaviour
{
    [Space(10)]
    [Header("Thing Properties")]
    //
    [SerializeField] private string thingName;
    [SerializeField] private int maxHealth = 100;

    public string ThingName => thingName;
    public int MaxHealth => maxHealth;
    public int CurrentHealth
    {
        get
        {
            return Mathf.Clamp(CurrentHealth, 0, maxHealth);
        }
        set
        {
            int previousHealth = CurrentHealth;

            CurrentHealth = Mathf.Clamp(value, 0, maxHealth);

            //effects management
            if (CurrentHealth == 0) DeathEffect();

            if (previousHealth > CurrentHealth) DamageEffect();
            else if (previousHealth < CurrentHealth) HealingEffect();
        }
    }

    #region Public methods

    /// <summary>
    /// Executes visible actions when the living thing dies.
    /// </summary>
    public virtual void DeathEffect()
    {
        Debug.Log($"{name}: i have died");
    }

    /// <summary>
    /// Executes visible actions when the living thing is damaged.
    /// </summary>
    public virtual void DamageEffect()
    {
        Debug.Log($"{name}: i have been damaged");
    }

    /// <summary>
    /// Executes visible actions when the living thing is healed.
    /// </summary>
    public virtual void HealingEffect()
    {
        Debug.Log($"{name}: i have been healed");
    }

    #endregion
}
