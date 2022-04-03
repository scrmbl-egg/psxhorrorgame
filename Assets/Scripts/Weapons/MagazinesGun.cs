using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinesGun : BaseWeapon, IWeapon, IGun
{
    [Header("Ammo Settings")]
    //
    [SerializeField] private int currentLoadedRounds;
    [SerializeField] private int maxMagazineCapacity;
    [SerializeField] private int currentAmountOfMagazines;
    [SerializeField] private int maxAmountOfMagazines;
    [Space(2)]
    [SerializeField] private List<int> magazines = new List<int>();

    public int CurrentLoadedRounds
    {
        get => currentLoadedRounds;
        set => currentLoadedRounds = Mathf.Clamp(value, 0, maxAmountOfMagazines);
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddMagazine(3);
        }
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
        bool theresNoBulletsInMag = CurrentLoadedRounds == 0;
        if (theresNoBulletsInMag) return;

        CurrentLoadedRounds--;
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
            Magazines.Add(CurrentLoadedRounds);
            
            //sorts list from least to greatest
            Magazines.Sort();

            int lastMemberOfList = Magazines.Count - 1;

            CurrentLoadedRounds = Magazines[lastMemberOfList];
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
        if (magazineListIsFull) Magazines.RemoveAt(0);

        Magazines.Add(ammo);
    }

    #endregion
}