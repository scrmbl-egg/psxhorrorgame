using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private string weaponName;

    [Space(10)]
    [Header("Properties")]
    //
    [SerializeField] private string ammoName;
    [SerializeField, Range(1, 20)] private int amountOfBullets;


    #region MonoBehaviour


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

        //TODO: LOOP THROUGH WEAPONS AND GIVE AMMO
    }

    #endregion
}
