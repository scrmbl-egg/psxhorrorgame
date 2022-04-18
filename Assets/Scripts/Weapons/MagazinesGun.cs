using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinesGun : BaseWeapon, IWeapon, IGun
{
    [Space(10)]
    [Header("Ammunition Settings")]
    //
    [SerializeField] private int currentLoadedRounds;
    [SerializeField, Min(1)] private int maxMagazineCapacity;
    [SerializeField] private int currentAmountOfMagazines;
    [SerializeField, Min(1)] private int maxAmountOfMagazines;
    [Space(2)]
    [SerializeField] private List<int> magazines = new List<int>();
    private const int ONE_IN_THE_CHAMBER = 1;

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set => currentLoadedRounds = Mathf.Clamp(value, 0, maxMagazineCapacity + ONE_IN_THE_CHAMBER);
    }
    public int MaxMagazineCapacity => maxMagazineCapacity;
    public int CurrentAmountOfMagazines
    {
        get => currentAmountOfMagazines;
        set => currentAmountOfMagazines = Mathf.Clamp(value, 0, maxAmountOfMagazines);
    }
    public int MaxAmountOfMagazines => maxAmountOfMagazines;
    public List<int> Magazines
    {
        get => magazines;
        set => magazines = value;
    }

    public bool IsAiming { get; set; }
    public bool IsCovering { get; set; }

    #region MonoBehaviour

    private void Awake()
    {
        Magazines.Capacity = MaxAmountOfMagazines;

        //TODO: Remove method when guns are completely coded
        FillGunWithRandomMags();
    }

    #endregion

    #region Public methods

    #region IWeapon

    public virtual void MeleeAttack()
    {
        //meelee attack
        Debug.Log($"{WeaponName}: Meelee attack");
    }

    public virtual void Fire()
    {
        bool thereAreBulletsInMag = CurrentLoadedRounds > 0;
        if (thereAreBulletsInMag)
        {
            for (int i = 0; i < PelletsPerShot; i++)
            {
                Ray shot = new Ray(origin: RaycastOrigin.position,
                                   direction: RandomSpread(RaycastOrigin.forward));
                bool objectIsNotInRange = !Physics.Raycast(ray: shot,
                                                           hitInfo: out RaycastHit hit,
                                                           maxDistance: WeaponRange,
                                                           layerMask: RaycastLayers);

                if (objectIsNotInRange) return;
                //else...

                bool livingThingIsHit = hit.transform.TryGetComponent(out LivingThing target);
                if (livingThingIsHit)
                {
                    target.Health -= WeaponDamage / PelletsPerShot;
                }
                else
                {
                    SpawnRandomBulletHole(hit);
                    PushRigidbodyFromRaycastHit(hit, WeaponForce / PelletsPerShot);
                }
            }

            CurrentLoadedRounds--;
        }
        else
        {
            //play an empty mag sound
        }
    }

    #endregion
    #region IGun

    public virtual void Reload()
    {
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

                if (magHasMoreThanOneRound) Magazines.Add(CurrentLoadedRounds - ONE_IN_THE_CHAMBER);
            }
            
            //sorts list from least to greatest
            Magazines.Sort();

            //loads magazine with most rounds
            int lastMemberOfList = Magazines.Count - 1;

            if (theresStillRoundsInMag) CurrentLoadedRounds = Magazines[lastMemberOfList] + ONE_IN_THE_CHAMBER;
            else CurrentLoadedRounds = Magazines[lastMemberOfList];
            
            Magazines.RemoveAt(lastMemberOfList);
        }
        else
        {
            //you can't reload
        }
    }

    public virtual void CheckAmmo()
    {
        Debug.Log($"AMMO: {CurrentLoadedRounds} | {Magazines.Count} magazines");
    }

    #endregion

    public void AddMagazine(int bulletAmount)
    {
        //sorts list from least to greatest.
        Magazines.Sort();

        bool magazineListIsFull = Magazines.Count == Magazines.Capacity;
        if (magazineListIsFull) Magazines.RemoveAt(0);

        int ammo = Mathf.Clamp(bulletAmount, 0, MaxMagazineCapacity);

        Magazines.Add(ammo);
        Debug.Log($"added magazine with {ammo} rounds");
    }

    #endregion
    #region Private methods

    private void FillGunWithRandomMags()
    {
        bool magsShouldBeSpawned = CurrentAmountOfMagazines <= 0;
        if (magsShouldBeSpawned) return;

        for (int i = 0; i < CurrentAmountOfMagazines; i++)
        {
            int randomAmountOfBullets = Random.Range(1, MaxMagazineCapacity + 1);
            Magazines.Add(randomAmountOfBullets);
        }
    }

    #endregion
}