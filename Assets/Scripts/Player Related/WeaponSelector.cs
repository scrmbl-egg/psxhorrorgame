using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header("Dependencies")]
    //
    [SerializeField] private PlayerMovement player;

    [Header("Weapons")]
    //
    [SerializeField] private int selectedWeapon;
    [SerializeField] private List<BaseWeapon> weaponList = new List<BaseWeapon>();

    #region MonoBehaviour
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        GetWeaponsInWeaponsList();
    }

    private void OnEnable()
    {
        _playerInputActions.PlayerThing.Aim.Enable();
        _playerInputActions.PlayerThing.Fire.Enable();
        _playerInputActions.PlayerThing.Reload.Enable();
        _playerInputActions.PlayerThing.CheckAmmo.Enable();
        _playerInputActions.PlayerThing.NavigateGuns.Enable();
        _playerInputActions.PlayerThing.Cover.Enable();

        _playerInputActions.PlayerThing.Aim.performed += AimDetection;
        _playerInputActions.PlayerThing.Aim.canceled += AimDetection;
        _playerInputActions.PlayerThing.Fire.started += FireDetection;
        _playerInputActions.PlayerThing.Reload.performed += ReloadDetection;
        _playerInputActions.PlayerThing.CheckAmmo.performed += CheckAmmoDetection;
        _playerInputActions.PlayerThing.Cover.started += CoverDetection;
        _playerInputActions.PlayerThing.Cover.canceled += CoverDetection;
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Aim.Disable();
        _playerInputActions.PlayerThing.Fire.Disable();
        _playerInputActions.PlayerThing.Reload.Disable();
        _playerInputActions.PlayerThing.CheckAmmo.Disable();
        _playerInputActions.PlayerThing.NavigateGuns.Disable();
        _playerInputActions.PlayerThing.Cover.Disable();

        _playerInputActions.PlayerThing.Aim.performed -= AimDetection;
        _playerInputActions.PlayerThing.Aim.canceled -= AimDetection;
        _playerInputActions.PlayerThing.Fire.started -= FireDetection;
        _playerInputActions.PlayerThing.Reload.performed -= ReloadDetection;
        _playerInputActions.PlayerThing.CheckAmmo.performed -= CheckAmmoDetection;
        _playerInputActions.PlayerThing.Cover.started -= CoverDetection;
        _playerInputActions.PlayerThing.Cover.canceled -= CoverDetection;
    }

    private void Update()
    {
        WeaponNavigation();
    }

    #endregion

    #region Private methods

    #region Input

    private void AimDetection(InputAction.CallbackContext ctx)
    {
        //player won't be able to aim if he is running

        bool selectedWeaponHasWeaponInterface = weaponList[selectedWeapon].TryGetComponent(out IWeapon weapon);
        bool isAbleToAim = !player.IsRunning && selectedWeaponHasWeaponInterface;

        if (ctx.performed && isAbleToAim) weapon.IsAiming = true;
        if (ctx.canceled || !isAbleToAim) weapon.IsAiming = false;
    }

    private void FireDetection(InputAction.CallbackContext ctx)
    {
        //player must be aiming in order to shoot, if he's not aiming, then do a melee attack

        bool selectedWeaponHasWeaponInterface = weaponList[selectedWeapon].TryGetComponent(out IWeapon weapon);
        bool isAbleToShoot = !player.IsRunning && selectedWeaponHasWeaponInterface && weapon.IsAiming;
        bool isAbleToMelee = !player.IsRunning && selectedWeaponHasWeaponInterface;

        if (ctx.started && isAbleToShoot) weapon.Fire();
        else if (ctx.started && isAbleToMelee) weapon.MeleeAttack();
    }

    private void ReloadDetection(InputAction.CallbackContext ctx)
    {
        ///player can't run and reload at the same time.
        ///player must stop running in order to reload.

        bool selectedWeaponHasGunInterface = weaponList[selectedWeapon].TryGetComponent(out IGun gun);
        bool isAbleToReload = !player.IsRunning && selectedWeaponHasGunInterface;

        if (ctx.performed && isAbleToReload) gun.Reload();
    }

    private void CheckAmmoDetection(InputAction.CallbackContext ctx)
    {
        ///player can't run and check ammo at the same time.
        ///player must stop running in order to check ammo.
        bool selectedWeaponHasGunInterface = weaponList[selectedWeapon].TryGetComponent(out IGun gun);
        bool isAbleToCheckAmmo = !player.IsRunning && selectedWeaponHasGunInterface;

        if (ctx.performed && isAbleToCheckAmmo) gun.CheckAmmo();
    }

    private void CoverDetection(InputAction.CallbackContext ctx)
    {
        ///player can't run and cover at the same time.
        ///player must stop running in order to cover himself.
        ///covering disables the ability to aim, shoot, reload, and check ammo

        bool selectedWeaponHasWeaponInterface = weaponList[selectedWeapon].TryGetComponent(out IWeapon weapon);
        bool isAbleToCover = !player.IsRunning && selectedWeaponHasWeaponInterface;

        if (ctx.started && isAbleToCover) weapon.IsCovering = true;
        else if (ctx.canceled || !isAbleToCover) weapon.IsCovering = false;
    }

    #endregion

    private void GetWeaponsInWeaponsList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool childHasBaseWeapon = transform.GetChild(i).TryGetComponent(out BaseWeapon weapon);
            if (childHasBaseWeapon) weaponList.Add(weapon);
        }
    }

    private void WeaponNavigation()
    {
        float input = _playerInputActions.PlayerThing.NavigateGuns.ReadValue<float>();

        if (input > 0)
        {
            //select next weapon
            selectedWeapon++;
            if (selectedWeapon >= weaponList.Count) selectedWeapon = 0;
        }
        else if (input < 0)
        {
            //select previous weapon
            selectedWeapon--;
            if (selectedWeapon < 0) selectedWeapon = weaponList.Count - 1;
        }

        //cycle through weapons and activate the one selected
        weaponList[selectedWeapon].gameObject.SetActive(true);
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (i != selectedWeapon) weaponList[i].gameObject.SetActive(false);
        }
    }

    #endregion
}
