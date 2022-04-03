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

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set => currentLoadedRounds = Mathf.Clamp(value, 0, maxLoadedRounds);
    }
    public int MaxLoadedRounds => maxLoadedRounds;
    public int CurrentTotalAmmo
    {
        get => currentTotalAmmo;
        set => currentTotalAmmo = Mathf.Clamp(value, 0, maxTotalAmmo);
    }
    public int MaxTotalAmmo => maxTotalAmmo;

    #region Public methods

    #region IWeapon

    public virtual void MeleeAttack()
    {
        //melee attack
    }

    public virtual void Fire()
    {
        bool gunIsEmpty = CurrentLoadedRounds == 0;
        if (gunIsEmpty)
        {
            //play empty sound

            return;
        }
    }

    #endregion
    #region IGun

    public virtual void Reload()
    {
        bool noAmmoLeft = CurrentTotalAmmo == 0;
        if (noAmmoLeft) return;
    }

    public virtual void CheckAmmo()
    {
        Debug.Log($"AMMO: {CurrentLoadedRounds} | {CurrentTotalAmmo}");
    }

    #endregion

    #endregion
}