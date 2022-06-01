using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for guns that are loaded with individual
/// rounds rather than being loaded with magazines.
/// </summary>
public class RoundsGun : BaseWeapon, IWeapon, IGun
{
    private bool _isAiming;

    [Space( 10 )]
    [Header( "Recoil Settings" )]
    //
    [SerializeField] private float recoilSpeed;
    [SerializeField] private float recoverSpeed;
    [SerializeField] private Vector3 recoil;
    private static CamShake _camShake;

    [Space( 10 )]
    [Header( "Ammunition Settings" )]
    //
    [SerializeField] private bool hasInfiniteAmmo;
    [SerializeField] private int currentLoadedRounds;
    [SerializeField, Min( 1 )] private int maxLoadedRounds;
    [SerializeField] private int currentTotalAmmo;
    [SerializeField, Min( 1 )] private int maxTotalAmmo;
    private const int ONE_IN_THE_CHAMBER = 1;

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set
        {
            if (hasInfiniteAmmo) return;
            //else...

            currentLoadedRounds = Mathf.Clamp( value, 0, maxLoadedRounds + ONE_IN_THE_CHAMBER );
        }
    }
    public int MaxLoadedRounds => maxLoadedRounds;
    public int CurrentTotalAmmo
    {
        get => currentTotalAmmo;
        set => currentTotalAmmo = Mathf.Clamp( value, 0, maxTotalAmmo );
    }
    public int MaxTotalAmmo => maxTotalAmmo;

    public bool IsAiming
    {
        get => _isAiming;
        set
        {
            _isAiming = value;
            AnimatorController.SetBool( "Aim", value );
        }
    }

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();

        if (_camShake == null) _camShake = GetComponentInParent<CamShake>();
    }

    private void Update()
    {
        UpdateAnimatorParameters();
    }

    #endregion

    #region Public methods

    #region IWeapon

    public virtual void MeleeAttack()
    {

        bool animatorIsNotIdleState = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" );

        if (animatorIsNotIdleState) return;
        //else...

        AnimatorController.SetTrigger( "Melee" );

        _camShake.ShakeCamera();

        Ray meleeRay =
            new Ray( origin: RaycastOrigin.position,
                    direction: RaycastOrigin.forward );
        bool objectIsNotInRange =
            !Physics.Raycast( ray: meleeRay,
                             hitInfo: out RaycastHit hit,
                             maxDistance: MeleeRange,
                             layerMask: AttackLayers,
                             queryTriggerInteraction: QueryTriggerInteraction.Ignore );

        if (objectIsNotInRange) return;
        //else...

        PushRigidbodyFromRaycastHit( hit, MeleeForce );

        bool hitDoesntHaveLivingThingsTag = !hit.transform.CompareTag( "LivingThings" );
        if (hitDoesntHaveLivingThingsTag) return;
        //else...

        GetLivingThingFromTransform( hit.transform, out LivingThing target );
        target.Health -= MeleeDamage;
        target.BleedFromRaycastHit( hit );
    }

    public virtual void Fire()
    {
        bool animatorIsNotAiming = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Aim" );
        bool gunHasAmmo = CurrentLoadedRounds > 0;

        if (animatorIsNotAiming) return;
        //else...

        if (gunHasAmmo)
        {
            AnimatorController.SetTrigger( "Shoot" );

            for (int i = 0; i < PelletsPerShot; i++)
            {
                Ray shot =
                    new Ray( origin: RaycastOrigin.position,
                            direction: ApplySpreadToDirection( RaycastOrigin.forward ) );
                bool objectIsNotInRange =
                    !Physics.Raycast( ray: shot,
                                     hitInfo: out RaycastHit hit,
                                     maxDistance: WeaponRange,
                                     layerMask: AttackLayers,
                                     QueryTriggerInteraction.Ignore );

                if (objectIsNotInRange) continue;
                //else...

                PushRigidbodyFromRaycastHit( hit, WeaponForce / PelletsPerShot );

                bool targetHasLivingThingTag = hit.transform.CompareTag( "LivingThings" );
                if (targetHasLivingThingTag)
                {
                    GetLivingThingFromTransform( hit.transform, out LivingThing target );

                    target.Health -= WeaponDamage / PelletsPerShot;
                    target.BleedFromRaycastHit( hit );
                }
                else
                {
                    SpawnRandomBulletHole( hit );
                }
            }

            CurrentLoadedRounds--;
            _camShake.ShakeCamera( recoil, recoilSpeed, recoverSpeed );
        }
        else
        {
            AnimatorController.SetTrigger( "FailedShot" );
        }
    }

    #endregion
    #region IGun

    public virtual void Reload()
    {
        bool theresAmmoLeft = CurrentTotalAmmo > 0;
        bool weaponIsNotFull = CurrentLoadedRounds < MaxLoadedRounds + ONE_IN_THE_CHAMBER;
        bool cantReload = !( theresAmmoLeft && weaponIsNotFull );

        bool animatorIsIdle = AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" );
        bool animatorIsReloading = AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "LoadedReload" );

        if (cantReload) return;
        //else...

        if (animatorIsIdle || animatorIsReloading)
        {
            bool gunHasLoadedRounds = CurrentLoadedRounds > 0;

            if (gunHasLoadedRounds)
            {
                AnimatorController.SetTrigger( "LoadedReload" );
            }
            else
            {
                AnimatorController.SetTrigger( "EmptyChamberReload" );
            }
        }
    }

    public virtual void CheckAmmo()
    {
        string rounds = CurrentTotalAmmo switch
        {
            1 => "round",
            _ => "rounds"
        };

        string message = hasInfiniteAmmo switch
        {
            false => $"{CurrentLoadedRounds} | {CurrentTotalAmmo} {rounds}",
            true => "INFINITE"
        };

        bool animatorIsNotIdle = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" );
        bool thereAreNoRounds = AnimatorController.GetInteger( "Rounds" ) <= 0;

        if (animatorIsNotIdle) return;
        //else...

        AmmoChecker.PrintMessage( message );

        if (thereAreNoRounds) return;
        //else...

        AnimatorController.SetTrigger( "CheckAmmo" );
    }

    #endregion

    public void AddRounds( int amount )
    {
        CurrentTotalAmmo += amount;
    }
    #region AnimationEvents

    public void AddRound()
    {
        CurrentLoadedRounds++;
        CurrentTotalAmmo--;
    }

    #endregion

    #endregion
    #region Private methods

    private void UpdateAnimatorParameters()
    {
        AnimatorController.SetInteger( "Rounds", CurrentLoadedRounds );
    }

    #endregion
}