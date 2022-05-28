using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinesGun : BaseWeapon, IWeapon, IGun
{
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
    [SerializeField, Min( 1 )] private int maxMagazineCapacity;
    [SerializeField, Min( 1 )] private int maxAmountOfMagazines;
    [Space( 2 )]
    [SerializeField] private List<int> magazines = new List<int>();
    private const int ONE_IN_THE_CHAMBER = 1;

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set
        {
            if (hasInfiniteAmmo) return;
            //else...

            currentLoadedRounds = Mathf.Clamp( value, 0, maxMagazineCapacity + ONE_IN_THE_CHAMBER );
        }
    }
    public int MaxMagazineCapacity => maxMagazineCapacity;
    public int MaxAmountOfMagazines => maxAmountOfMagazines;
    public List<int> Magazines
    {
        get => magazines;
        set => magazines = value;
    }

    private bool _isAiming;
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

        //TODO: Remove method when guns are completely coded
        FillGunWithRandomMags();
    }

    private void Update()
    {
        UpdateAnimatorParameters();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay( RaycastOrigin.position, RaycastOrigin.forward * MeleeRange );
    }

    #region Animation Events



    #endregion

    #endregion

    #region Public methods

    #region IWeapon

    public virtual void MeleeAttack()
    {
        bool animatorIsNotIdle = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" );
        if (animatorIsNotIdle) return;
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
        bool cantFire = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsTag( "CanFire" );
        bool thereAreBulletsInMag = CurrentLoadedRounds > 0;

        if (cantFire) return;
        //else...

        if (thereAreBulletsInMag)
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
                                     queryTriggerInteraction: QueryTriggerInteraction.Ignore );

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

            bool lastRound = CurrentLoadedRounds == 1;
            if (lastRound)
            {
                AnimatorController.SetLayerWeight( 1, 1 );
            }

            CurrentLoadedRounds--;
            _camShake.ShakeCamera( recoil, recoilSpeed, recoverSpeed );
        }
        else
        {
            AnimatorController.SetTrigger( "FailedShot" );

            //play an empty mag sound
        }
    }

    #endregion
    #region IGun

    public virtual void Reload()
    {
        bool animatorIsNotIdle = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" );
        if (animatorIsNotIdle) return;
        //else...

        bool thereAreMagazinesAvailable = Magazines.Count > 0;
        if (thereAreMagazinesAvailable)
        {
            //extract current mag and add it to ammo list
            bool theresStillRoundsInMag = CurrentLoadedRounds > 0;
            bool magHasMoreThanOneRound = CurrentLoadedRounds - ONE_IN_THE_CHAMBER > 0;

            if (theresStillRoundsInMag)
            {
                ///keep one round in the chamber
                ///save the used mag in pocket

                if (magHasMoreThanOneRound) Magazines.Add( CurrentLoadedRounds - ONE_IN_THE_CHAMBER );

                AnimatorController.SetTrigger( "NoRackReload" );
            }
            else
            {
                AnimatorController.SetLayerWeight( 1, 0 );
                AnimatorController.SetTrigger( "RackReload" );
            }

            //sorts list from least to greatest
            Magazines.Sort();

            //loads magazine with most rounds
            int lastMemberOfList = Magazines.Count - 1;

            if (theresStillRoundsInMag)
            {
                CurrentLoadedRounds = Magazines[ lastMemberOfList ] + ONE_IN_THE_CHAMBER;
            }
            else
            {
                CurrentLoadedRounds = Magazines[ lastMemberOfList ];
            }

            Magazines.RemoveAt( lastMemberOfList );
        }
        else
        {
            //you can't reload
        }
    }

    public virtual void CheckAmmo()
    {
        bool animatorIsIdle = !AnimatorController.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" );
        bool thereAreNoRounds = AnimatorController.GetInteger( "Rounds" ) <= 0;
        string mags = Magazines.Count switch
        {
            1 => "mag",
            _ => "mags"
        };
        string message = hasInfiniteAmmo switch
        {
            false => $"{CurrentLoadedRounds} | {Magazines.Count} {mags}",
            true => "INFINITE"
        };

        if (animatorIsIdle) return;
        //else...

        AmmoChecker.PrintMessage( message );

        if (thereAreNoRounds) return;
        //else...

        AnimatorController.SetTrigger( "CheckAmmo" );
    }

    #endregion

    public void AddMagazine( int bulletAmount )
    {
        bool magazineListIsFull = Magazines.Count >= MaxAmountOfMagazines;

        Magazines.Sort();

        if (magazineListIsFull)
        {
            Magazines.RemoveAt( 0 );
        }

        Magazines.Add( bulletAmount );
    }

    #endregion
    #region Private methods

    private void UpdateAnimatorParameters()
    {
        AnimatorController.SetInteger( "Rounds", CurrentLoadedRounds );
        AnimatorController.SetInteger( "Magazines", Magazines.Count );
    }

    private void FillGunWithRandomMags()
    {
        bool magsShouldntBeSpawned = Magazines.Count > 0;
        if (magsShouldntBeSpawned) return;
        //else...

        for (int i = 0; i < MaxAmountOfMagazines; i++)
        {
            int randomAmountOfBullets = Random.Range( 0, MaxMagazineCapacity ) + 1;
            Magazines.Add( randomAmountOfBullets );
        }
    }

    #endregion
}