using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LivingThing : MonoBehaviour
{
    [Space(10)]
    [Header("Thing Properties")]
    //
    [SerializeField] private string thingName;
    [SerializeField] private bool isInvincible;
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
            bool isAboutToDie = health == 0 && !isInvincible;
            bool hasLostHealth = previousHealth > health;
            bool hasGainedHealth = previousHealth < health;

            health = Mathf.Clamp(value, 0, maxHealth);

            if (isAboutToDie) DeathEffect();

            if (hasLostHealth) DamageEffect();
            else if (hasGainedHealth) HealingEffect();

            //heal back if invincible
            if (isInvincible) health = previousHealth;
        }
    }

    [Space(10)]
    [Header("Effects")]
    //
    [SerializeField] private GameObject[] bloodPrefabs;

    #region MonoBehaviour

    private void Awake()
    {
        health = MaxHealth;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Executes visible actions when the living thing dies. Example: instantiating a ragdoll.
    /// </summary>
    public virtual void DeathEffect()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Executes visible actions when the living thing is damaged. Example: animations, sounds...
    /// </summary>
    public virtual void DamageEffect()
    {

    }

    /// <summary>
    /// Executes visible actions when the living thing is healed. Example: animations, general particle effects...
    /// </summary>
    public virtual void HealingEffect()
    {

    }

    /// <summary>
    /// Instantiates a blood particle system from a RaycastHit.
    /// </summary>
    /// <param name="hitInfo"></param>
    public void BleedFromRaycastHit(RaycastHit hitInfo)
    {
        bool bloodPrefabIsNull = bloodPrefabs == null;
        if (bloodPrefabIsNull) return;
        //else...

        Vector3 position = hitInfo.point;
        Quaternion rotation = Quaternion.LookRotation(hitInfo.normal);
        int random = Random.Range(0, bloodPrefabs.Length);

        GameObject bloodGameObject =
            Instantiate(
                original: bloodPrefabs[random],
                position: position,
                rotation: rotation);

        bool bloodDoesntHaveParticleSystem = !bloodGameObject.TryGetComponent(out ParticleSystem particles);
        if (bloodDoesntHaveParticleSystem) return;
        //else...

        particles.Play();
    }

    /// <summary>
    /// Instantiates a blood particle system in the center of the thing.
    /// </summary>
    public void Bleed()
    {
        bool bloodPrefabIsNull = bloodPrefabs == null;
        if (bloodPrefabIsNull) return;
        //else...

        Vector3 position = transform.position;
        Quaternion rotation = Quaternion.identity;
        int random = Random.Range(0, bloodPrefabs.Length);

        GameObject bloodGameObject =
            Instantiate(
                original: bloodPrefabs[random],
                position: position,
                rotation: rotation);

        bool bloodDoesntHaveParticleSystem = !bloodGameObject.TryGetComponent(out ParticleSystem particles);
        if (bloodDoesntHaveParticleSystem) return;
        //else...

        particles.Play();
    }

    #endregion
    #region Private methods

    #endregion
}