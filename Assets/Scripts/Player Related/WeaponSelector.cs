using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    //input
    PlayerInputActions _playerInputActions;

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

        _playerInputActions.PlayerThing.Aim.performed += AimDetection;
        _playerInputActions.PlayerThing.Aim.canceled += AimDetection;
        _playerInputActions.PlayerThing.Fire.started += FireDetection;
        _playerInputActions.PlayerThing.Reload.performed += ReloadDetection;
        _playerInputActions.PlayerThing.CheckAmmo.performed += CheckAmmoDetection;
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Aim.Disable();
        _playerInputActions.PlayerThing.Fire.Disable();
        _playerInputActions.PlayerThing.Reload.Disable();
        _playerInputActions.PlayerThing.CheckAmmo.Disable();
        _playerInputActions.PlayerThing.NavigateGuns.Disable();

        _playerInputActions.PlayerThing.Aim.performed -= AimDetection;
        _playerInputActions.PlayerThing.Aim.canceled -= AimDetection;
        _playerInputActions.PlayerThing.Fire.started -= FireDetection;
        _playerInputActions.PlayerThing.Reload.performed -= ReloadDetection;
        _playerInputActions.PlayerThing.CheckAmmo.performed -= CheckAmmoDetection;
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
        bool selectedWeaponHasWeaponInterface = weaponList[selectedWeapon].TryGetComponent(out IWeapon weapon);

        if (ctx.performed && selectedWeaponHasWeaponInterface) weapon.IsAiming = true;
        if (ctx.canceled && selectedWeaponHasWeaponInterface) weapon.IsAiming = false;
    }

    private void FireDetection(InputAction.CallbackContext ctx)
    {
        bool selectedWeaponHasWeaponInterface = weaponList[selectedWeapon].TryGetComponent(out IWeapon weapon);

        if (ctx.started && selectedWeaponHasWeaponInterface && weapon.IsAiming) weapon.Fire();
    }

    private void ReloadDetection(InputAction.CallbackContext ctx)
    {
        bool selectedWeaponHasGunInterface = weaponList[selectedWeapon].TryGetComponent(out IGun gun);

        if (ctx.performed && selectedWeaponHasGunInterface) gun.Reload();
    }

    private void CheckAmmoDetection(InputAction.CallbackContext ctx)
    {
        bool selectedWeaponHasGunInterface = weaponList[selectedWeapon].TryGetComponent(out IGun gun);

        if (ctx.performed && selectedWeaponHasGunInterface) gun.CheckAmmo();
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
            selectedWeapon++;
            if (selectedWeapon >= weaponList.Count) selectedWeapon = 0;
        }
        else if (input < 0)
        {
            selectedWeapon--;
            if (selectedWeapon < 0) selectedWeapon = weaponList.Count - 1;
        }

        weaponList[selectedWeapon].gameObject.SetActive(true);
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (i != selectedWeapon) weaponList[i].gameObject.SetActive(false);
        }
    }

    #endregion
}
