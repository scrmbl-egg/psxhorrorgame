using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private string targetWeaponName;

    [Space(10)]
    [Header("Properties")]
    //
    [SerializeField] private string ammoName;
    [SerializeField] private int amountOfBullets;

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

        for (int i = 0; i < currentWeapons.Count; i++)
        {
            bool weaponNamesAreNotTheSame = currentWeapons[i].WeaponName != targetWeaponName;

            if (weaponNamesAreNotTheSame) continue;
            //else...

            bool weaponHasMagazineGunClass = currentWeapons[i].TryGetComponent(out MagazinesGun mGun);
            bool weaponHasRoundsGunClass = currentWeapons[i].TryGetComponent(out RoundsGun rGun);

            if (weaponHasMagazineGunClass) mGun.AddMagazine(amountOfBullets);
            else if (weaponHasRoundsGunClass) rGun.AddRounds(amountOfBullets);

            Destroy(gameObject);
        }
    }

    #endregion
}