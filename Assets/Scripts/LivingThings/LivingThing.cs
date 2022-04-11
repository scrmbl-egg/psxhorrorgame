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
    [SerializeField] private int health;

    public string ThingName => thingName;
    public int MaxHealth => maxHealth;
    public int Health
    {
        get => Mathf.Clamp(health, 0, maxHealth);
        set
        {
            int previousHealth = health;

            health = Mathf.Clamp(value, 0, maxHealth);

            //effects management
            if (health == 0) DeathEffect();

            if (previousHealth > health) DamageEffect();
            else if (previousHealth < health) HealingEffect();
        }
    }

    #region MonoBehaviour

    private void Awake()
    {
        health = MaxHealth;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Executes visible actions when the living thing dies.
    /// </summary>
    public virtual void DeathEffect()
    {
        Debug.Log($"{ThingName}: i have died");
        Destroy(gameObject);
    }

    /// <summary>
    /// Executes visible actions when the living thing is damaged.
    /// </summary>
    public virtual void DamageEffect()
    {
        Debug.Log($"{ThingName}: i have been damaged");
    }

    /// <summary>
    /// Executes visible actions when the living thing is healed.
    /// </summary>
    public virtual void HealingEffect()
    {
        Debug.Log($"{ThingName}: i have been healed");
    }

    #endregion
}
