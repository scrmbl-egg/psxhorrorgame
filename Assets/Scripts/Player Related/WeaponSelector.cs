using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header( "Dependencies" )]
    //
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    private bool _isPressingAim;

    [Header( "Weapons" )]
    //
    [SerializeField] private int selectedWeapon;
    [SerializeField] private List<BaseWeapon> weaponList;

    public List<BaseWeapon> WeaponList => weaponList;

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
        _playerInputActions.PlayerThing.SelectWeapon1.Enable();
        _playerInputActions.PlayerThing.SelectWeapon2.Enable();

        _playerInputActions.PlayerThing.Aim.performed += AimDetection;
        _playerInputActions.PlayerThing.Aim.canceled += AimDetection;
        _playerInputActions.PlayerThing.Fire.started += FireDetection;
        _playerInputActions.PlayerThing.Reload.performed += ReloadDetection;
        _playerInputActions.PlayerThing.CheckAmmo.performed += CheckAmmoDetection;
        _playerInputActions.PlayerThing.SelectWeapon1.started += SelectWeapon1Detection;
        _playerInputActions.PlayerThing.SelectWeapon2.started += SelectWeapon2Detection;
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Aim.Disable();
        _playerInputActions.PlayerThing.Fire.Disable();
        _playerInputActions.PlayerThing.Reload.Disable();
        _playerInputActions.PlayerThing.CheckAmmo.Disable();
        _playerInputActions.PlayerThing.NavigateGuns.Disable();
        _playerInputActions.PlayerThing.Cover.Disable();
        _playerInputActions.PlayerThing.SelectWeapon1.Disable();
        _playerInputActions.PlayerThing.SelectWeapon2.Disable();

        _playerInputActions.PlayerThing.Aim.performed -= AimDetection;
        _playerInputActions.PlayerThing.Aim.canceled -= AimDetection;
        _playerInputActions.PlayerThing.Fire.started -= FireDetection;
        _playerInputActions.PlayerThing.Reload.performed -= ReloadDetection;
        _playerInputActions.PlayerThing.CheckAmmo.performed -= CheckAmmoDetection;
        _playerInputActions.PlayerThing.SelectWeapon1.started -= SelectWeapon1Detection;
        _playerInputActions.PlayerThing.SelectWeapon2.started -= SelectWeapon2Detection;
    }

    private void Update()
    {
        ManageAim();
        WeaponNavigation();
    }

    #endregion

    #region Private methods

    #region Input

    private void AimDetection( InputAction.CallbackContext ctx )
    {
        bool isPressingAim = ctx.ReadValueAsButton();
        _isPressingAim = isPressingAim;
    }

    private void FireDetection( InputAction.CallbackContext ctx )
    {
        //player must be aiming in order to shoot, if he's not aiming, then do a melee attack

        bool selectedWeaponHasWeaponInterface = weaponList[ selectedWeapon ].TryGetComponent( out IWeapon weapon );
        bool isAbleToShoot = !playerMovement.IsRunning && selectedWeaponHasWeaponInterface && weapon.IsAiming;
        bool isAbleToMelee = !playerMovement.IsRunning && selectedWeaponHasWeaponInterface;

        if (ctx.started && isAbleToShoot) weapon.Fire();
        else if (ctx.started && isAbleToMelee) weapon.MeleeAttack();
    }

    private void ReloadDetection( InputAction.CallbackContext ctx )
    {
        ///player can't run and reload at the same time.
        ///player must stop running in order to reload.

        bool selectedWeaponHasGunInterface = weaponList[ selectedWeapon ].TryGetComponent( out IGun gun );
        bool isAbleToReload = !playerMovement.IsRunning && selectedWeaponHasGunInterface;

        if (ctx.performed && isAbleToReload) gun.Reload();
    }

    private void CheckAmmoDetection( InputAction.CallbackContext ctx )
    {
        ///player can't run and check ammo at the same time.
        ///player must stop running in order to check ammo.
        bool selectedWeaponHasGunInterface = weaponList[ selectedWeapon ].TryGetComponent( out IGun gun );
        bool isAbleToCheckAmmo = !playerMovement.IsRunning && selectedWeaponHasGunInterface;

        if (ctx.performed && isAbleToCheckAmmo) gun.CheckAmmo();
    }

    private void SelectWeapon1Detection( InputAction.CallbackContext ctx )
    {
        if (ctx.started) SelectWeapon( 1 );
    }

    private void SelectWeapon2Detection(InputAction.CallbackContext ctx )
    {
        if (ctx.started) SelectWeapon( 2 );
    }

    #endregion

    private void ManageAim()
    {
        bool canAim = _isPressingAim && !playerMovement.IsRunning;
        bool selectedWeaponDoesntHaveWeaponInterface = !weaponList[ selectedWeapon ].TryGetComponent( out IWeapon weapon );

        if (selectedWeaponDoesntHaveWeaponInterface) return;
        //else...

        weapon.IsAiming = canAim;
        playerLook.IsAiming = canAim;
    }

    private void GetWeaponsInWeaponsList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool childHasBaseWeapon = transform.GetChild( i ).TryGetComponent( out BaseWeapon weapon );
            if (childHasBaseWeapon) weaponList.Add( weapon );
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
        weaponList[ selectedWeapon ].gameObject.SetActive( true );
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (i != selectedWeapon) weaponList[ i ].gameObject.SetActive( false );
        }
    }

    private void SelectWeapon( int inputNumber )
    {
        int index = inputNumber - 1;

        if (index == selectedWeapon) return;
        //else...

        selectedWeapon = index;
        weaponList[ selectedWeapon ].gameObject.SetActive( true );
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (i != selectedWeapon) weaponList[ i ].gameObject.SetActive( false );
        }
    }

    #endregion
}
