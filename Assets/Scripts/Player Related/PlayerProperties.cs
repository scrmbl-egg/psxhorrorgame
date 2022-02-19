using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour, IDamageable
{
    [Header("Propiedades")]
    //
    [SerializeField] int maxPlayerHealth = 100;
    public int PlayerHealth
    {
        get
        {
            return Mathf.Clamp(PlayerHealth, 0, maxPlayerHealth);
        }
        set
        {
            PlayerHealth = Mathf.Clamp(value, 0, maxPlayerHealth);
        }
    }

    #region IDamageable
    public void Damage(int damage)
    {
        PlayerHealth -= damage;

        //TODO: Add effects when damaged
        //effects
    }

    public void Heal(int hp)
    {
        PlayerHealth += hp;

        //TODO: Add effects when healed
        //effects
    }

    public void Death()
    {
        //TODO: Code death effects
    }
    #endregion
}
