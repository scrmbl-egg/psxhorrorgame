using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic weapon class.
/// </summary>
public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    //
    [SerializeField] private string weaponName;
    [Space(2)]
    [SerializeField] private Transform raycastOrigin;
    [Space(2)]
    [SerializeField, Min(1)] private int pelletsPerShot = 1;
    [Space(2)]
    [SerializeField, Min(1)] private int meleeDamage;
    [SerializeField, Min(1)] private int minWeaponDamage;
    [SerializeField, Min(1)] private int maxWeaponDamage;
    [Space(2)]
    [SerializeField, Min(float.Epsilon)] private float meeleeRange;
    [SerializeField, Min(float.Epsilon)] private float meleeForce;
    [SerializeField, Min(float.Epsilon)] private float weaponRange;
    [SerializeField, Min(float.Epsilon)] private float weaponForce;
    [Space(2)]
    [SerializeField, Range(0f, 45f)] private float horizontalSpread;
    [SerializeField, Range(0f, 45f)] private float verticalSpread;
    [Space(2)]
    [SerializeField] private LayerMask attackLayers;
    [SerializeField] private GameObject[] hitDecals;
    [SerializeField] private GameObject[] hitParticles;
    private static AmmoChecker _ammoChecker;

    public string WeaponName => weaponName;
    public Transform RaycastOrigin => raycastOrigin;
    public int PelletsPerShot => pelletsPerShot;
    public int MeleeDamage => meleeDamage;
    public int WeaponDamage => Random.Range(minWeaponDamage, maxWeaponDamage + 1); 
    public float MeleeRange => meeleeRange;
    public float MeleeForce => meleeForce;
    public float WeaponRange => weaponRange;
    public float WeaponForce => weaponForce;
    public LayerMask AttackLayers => attackLayers;
    public GameObject RandomHitDecal 
    {
        get
        {
            int random = Random.Range(0, hitDecals.Length);
            return hitDecals[random];
        }
    }
    public GameObject RandomHitParticle
    {
        get
        {
            int random = Random.Range(0, hitParticles.Length);
            return hitParticles[random];
        }
    }
    public static AmmoChecker AmmoChecker => _ammoChecker;

    #region MonoBehaviour

    public virtual void Awake()
    {
        if (_ammoChecker == null) _ammoChecker = FindObjectOfType<AmmoChecker>();
    }

    #endregion

    #region Public methods

    public Vector3 ApplySpreadToDirection(Vector3 vector)
    {
        float hDegrees = horizontalSpread / 2;
        float vDegrees = verticalSpread / 2;

        float randomHorizontalSpread = Random.Range(-hDegrees, hDegrees);
        float randomVerticalSpread = Random.Range(-vDegrees, vDegrees);

        vector = Quaternion.AngleAxis(randomHorizontalSpread, RaycastOrigin.up) * vector; //horizontal spread
        vector = Quaternion.AngleAxis(randomVerticalSpread, RaycastOrigin.right) * vector; //vertical spread

        return vector;
    }

    public void SpawnRandomBulletHole(RaycastHit hitInfo)
    {
        if (hitDecals == null) return;
        //else...
        
        //spawn bullet hole
        GameObject bulletHole = Instantiate(original: RandomHitDecal,
                                            position: hitInfo.point,
                                            rotation: Quaternion.LookRotation(hitInfo.normal));

        bulletHole.transform.parent = hitInfo.transform;

        if (hitParticles == null) return;
        //else...

        //spawn particles
        GameObject debris = Instantiate(original: RandomHitParticle,
                                        position: hitInfo.point,
                                        rotation: Quaternion.LookRotation(hitInfo.normal));

        bool debrisDoesntHaveParticleSystem = !debris.TryGetComponent(out ParticleSystem particles);
        if (debrisDoesntHaveParticleSystem) return;
        //else...

        particles.Play();
    }

    public void PushRigidbodyFromRaycastHit(RaycastHit hitInfo, float force)
    {
        bool rigidbodyIsNotDetected = hitInfo.rigidbody == null;
        if (rigidbodyIsNotDetected) return;
        //else...

        Rigidbody targetRigidbody = hitInfo.rigidbody;
        Vector3 forceVector = -hitInfo.normal * force;

        bool rigidbodyIsNotKinematic = !targetRigidbody.isKinematic;
        if (rigidbodyIsNotKinematic) 
            targetRigidbody.AddForce(forceVector, ForceMode.Impulse);
    }

    public void GetLivingThingFromTransform(Transform transform, out LivingThing livingThing)
    {
        livingThing = null;

        bool parentIsNotLivingThing = true;
        while (parentIsNotLivingThing)
        {
            bool currentTransformIsNotLivingThing = !transform.TryGetComponent(out livingThing);

            if (currentTransformIsNotLivingThing)
                transform = transform.parent; //go to parent transform and reiterate loop from there
            else
                parentIsNotLivingThing = false; //end loop
        }
    }
    #endregion
}