using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ammunition : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private Collider interactionArea;
    [SerializeField] private string targetWeaponName;

    [Space(10)]
    [Header("Properties")]
    //
    [SerializeField] private string ammoName;
    [SerializeField, Min(1)] private int minimumBullets = 1;
    [SerializeField, Range(0, MAX_BONUS_BULLETS)] private int maxRandomBonusBullets;
    private const int MAX_BONUS_BULLETS = 10;
    public int AmountOfBullets
    {
        get
        {
            int bonus = Random.Range(0, MAX_BONUS_BULLETS + 1);

            return minimumBullets + bonus;
        }
    }

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool showInteractionArea;
    private static Color _gizmoColor = new Color(0.2f, 0.4f, 1, 0.5f);

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        if (showInteractionArea) DrawInteractionArea();
    }

    #endregion

    #region Public methods

    #region IInteractive

    public void Interact(Component sender)
    {
        GiveAmmoToWeapon(sender);
    }

    #endregion

    #endregion
    #region Private methods

    private void GiveAmmoToWeapon(Component sender)
    {
        bool senderDoesntHaveInventory = !sender.TryGetComponent(out PlayerInventory inventory);
        if (senderDoesntHaveInventory) return;
        //else...

        List<BaseWeapon> currentWeapons = inventory.WeaponSelector.WeaponList;

        foreach (BaseWeapon weapon in currentWeapons)
        {
            bool weaponNamesAreNotTheSame = weapon.WeaponName != targetWeaponName;

            if (weaponNamesAreNotTheSame) continue;
            //else...

            bool weaponHasMagazineGunClass = weapon.TryGetComponent(out MagazinesGun mGun);
            bool weaponHasRoundsGunClass = weapon.TryGetComponent(out RoundsGun rGun);

            if (weaponHasMagazineGunClass) mGun.AddMagazine(AmountOfBullets);
            if (weaponHasRoundsGunClass) rGun.AddRounds(AmountOfBullets);

            Destroy(gameObject);
        }
    }

    private void DrawInteractionArea()
    {
        Vector3 origin = interactionArea.bounds.center;
        Vector3 size = interactionArea.bounds.size;
        
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(origin, size);
    }

    #endregion
}