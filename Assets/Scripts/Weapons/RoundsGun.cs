using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for guns that are loaded with individual
/// rounds rather than being loaded with magazines.
/// </summary>
public class RoundsGun : BaseWeapon, IWeapon, IGun
{
    [Space(10)]
    [Header("Recoil Settings")]
    //
    [SerializeField] private float recoilSpeed;
    [SerializeField] private float recoverSpeed;
    [SerializeField] private Vector3 recoil;
    private CamShake _camShake;

    [Space(10)]
    [Header("Ammunition Settings")]
    //
    [SerializeField] private bool hasInfiniteAmmo;
    [SerializeField] private int currentLoadedRounds;
    [SerializeField, Min(1)] private int maxLoadedRounds;
    [SerializeField] private int currentTotalAmmo;
    [SerializeField, Min(1)] private int maxTotalAmmo;
    private const int ONE_IN_THE_CHAMBER = 1;

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set
        {
            if (hasInfiniteAmmo) return;
            //else...

            currentLoadedRounds = Mathf.Clamp(value, 0, maxLoadedRounds + ONE_IN_THE_CHAMBER);
        }
    }
    public int MaxLoadedRounds => maxLoadedRounds;
    public int CurrentTotalAmmo
    {
        get => currentTotalAmmo;
        set => currentTotalAmmo = Mathf.Clamp(value, 0, maxTotalAmmo);
    }
    public int MaxTotalAmmo => maxTotalAmmo;

    public bool IsAiming { get; set; }
    public bool IsCovering { get; set; }

    #region MonoBehaviour

    private void Awake()
    {
        _camShake = GetComponentInParent<CamShake>();
    }

    #endregion

    #region Public methods

    #region IWeapon

    public virtual void MeleeAttack()
    {
        //melee attack
    }

    public virtual void Fire()
    {
        bool gunHasAmmo = CurrentLoadedRounds > 0;
        if (gunHasAmmo)
        {
            for (int i = 0; i < PelletsPerShot; i++)
            {
                Ray shot = 
                    new Ray(origin: RaycastOrigin.position,
                            direction: ApplySpreadToDirection(RaycastOrigin.forward));
                bool objectIsNotInRange = 
                    !Physics.Raycast(ray: shot,
                                     hitInfo: out RaycastHit hit,
                                     maxDistance: WeaponRange,
                                     layerMask: RaycastLayers);

                if (objectIsNotInRange) continue;
                //else...

                PushRigidbodyFromRaycastHit(hit, WeaponForce / PelletsPerShot);

                bool targetHasLivingThingTag = hit.transform.CompareTag("LivingThings");
                if (targetHasLivingThingTag)
                {
                    GetLivingThingFromTransform(hit.transform, out LivingThing target);

                    target.Health -= WeaponDamage / PelletsPerShot;
                    target.BleedFromRaycastHit(hit);
                }
                else
                {
                    SpawnRandomBulletHole(hit);
                }
            }

            CurrentLoadedRounds--;
            _camShake.ShakeCamera(recoil, recoilSpeed, recoverSpeed);
        }
        else
        {
            //play empty sound
        }
    }

    #endregion
    #region IGun

    public virtual void Reload()
    {
        bool theresAmmoLeft = CurrentTotalAmmo > 0;
        bool weaponIsNotFull = CurrentLoadedRounds < MaxLoadedRounds + ONE_IN_THE_CHAMBER;

        if (theresAmmoLeft && weaponIsNotFull)
        {
            CurrentTotalAmmo--;
            CurrentLoadedRounds++;
        }
    }

    public virtual void CheckAmmo()
    {
        Debug.Log($"AMMO: {CurrentLoadedRounds} | {CurrentTotalAmmo}");
    }

    #endregion

    public void AddRounds(int amount)
    {
        CurrentTotalAmmo += amount;
    }

    #endregion
}