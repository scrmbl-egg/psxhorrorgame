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
    [Header("Ammunition Settings")]
    //
    [SerializeField] private int currentLoadedRounds;
    [SerializeField] private int maxLoadedRounds;
    [SerializeField] private int currentTotalAmmo;
    [SerializeField] private int maxTotalAmmo;
    private const int ONE_IN_THE_CHAMBER = 1;

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set => currentLoadedRounds = Mathf.Clamp(value, 0, maxLoadedRounds + ONE_IN_THE_CHAMBER);
    }
    public int MaxLoadedRounds => maxLoadedRounds;
    public int CurrentTotalAmmo
    {
        get => currentTotalAmmo;
        set => currentTotalAmmo = Mathf.Clamp(value, 0, maxTotalAmmo);
    }
    public int MaxTotalAmmo => maxTotalAmmo;

    public bool IsAiming { get; set; }

    #region Public methods

    #region IWeapon

    public virtual void MeleeAttack()
    {
        //melee attack
        Debug.Log($"{WeaponName}: Melee attack");
    }

    public virtual void Fire()
    {
        bool gunHasAmmo = CurrentLoadedRounds > 0;
        if (gunHasAmmo)
        {
            for (int i = 0; i < PelletsPerShot; i++)
            {
                Ray shot = new Ray(origin: RaycastOrigin.position,
                                   direction: RandomSpread(RaycastOrigin.forward));
                bool objectIsInRange = Physics.Raycast(ray: shot,
                                                       hitInfo: out RaycastHit hit,
                                                       maxDistance: WeaponRange,
                                                       layerMask: RaycastLayers);

                if (objectIsInRange)
                {
                    bool livingThingIsHit = hit.transform.TryGetComponent(out LivingThing target);
                    if (livingThingIsHit)
                    {
                        target.Health -= WeaponDamage / PelletsPerShot;
                    }
                    else
                    {
                        SpawnRandomBulletHole(hit);
                    }
                }
            }

            CurrentLoadedRounds--;
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

    #endregion
}