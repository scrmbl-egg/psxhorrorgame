using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinesGun : BaseWeapon, IWeapon, IGun
{
    [Space(10)]
    [Header("Ammunition Settings")]
    //
    [SerializeField] private int currentLoadedRounds;
    [SerializeField] private int maxMagazineCapacity;
    [SerializeField] private int currentAmountOfMagazines;
    [SerializeField] private int maxAmountOfMagazines;
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

    #region MonoBehaviour

    private void Awake()
    {
        Magazines.Capacity = MaxAmountOfMagazines;
    }

    private void Update()
    {
        InputManagement();
    }

    #endregion

    #region Public methods

    #region IWeapon

    [ContextMenu("Melee")]
    public virtual void MeleeAttack()
    {
        //meelee attack
    }

    [ContextMenu("Fire")]
    public virtual void Fire()
    {
        bool thereAreBulletsInMag = CurrentLoadedRounds > 0;
        if (thereAreBulletsInMag)
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
                    bool enemyIsHit = hit.transform.TryGetComponent(out EnemyThing enemy);
                    if (enemyIsHit)
                    {
                        enemy.Health -= WeaponDamage / PelletsPerShot;
                    }
                    else
                    {
                        //bullet hole
                        int randomBulletHole = Random.Range(0, BulletHoleDecals.Length);
                        GameObject bulletHole = Instantiate(original: BulletHoleDecals[randomBulletHole],
                                                            position: hit.point,
                                                            rotation: Quaternion.LookRotation(hit.normal));
                        bulletHole.transform.SetParent(hit.transform);
                    }
                }
            }

            CurrentLoadedRounds--;
        }
        else
        {
        }
    }

    #endregion
    #region IGun

    [ContextMenu("Reload")]
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

    [ContextMenu("Ammocheck")]
    public virtual void CheckAmmo()
    {
        Debug.Log($"ammo: {CurrentLoadedRounds} | {Magazines.Count}");
    }

    #endregion

    public void AddMagazine(int bulletAmount)
    {
        int ammo = Mathf.Clamp(bulletAmount, 0, MaxMagazineCapacity);

        //sorts list from least to greatest.
        Magazines.Sort();

        bool magazineListIsFull = Magazines.Count == Magazines.Capacity;
        if (magazineListIsFull)
        {
            Magazines.RemoveAt(0);
        }

        Magazines.Add(ammo);
        Debug.Log($"added magazine with {ammo} rounds");
    }

    #endregion
    #region Private methods

    private void InputManagement()
    {
        //TODO: USE INPUT MANAGER BUTTON INSTEAD OF KEYCODE ONCE FINISHED
        if (Input.GetKeyDown(KeyCode.R)) Reload();
        if (Input.GetKeyDown(KeyCode.F)) CheckAmmo();
        if (Input.GetButtonDown("Fire1")) Fire();
    }

    #endregion
}