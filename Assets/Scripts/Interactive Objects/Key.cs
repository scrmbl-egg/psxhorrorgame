using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractive
{
    [Header("Properties")]
    //
    [SerializeField, Range(1, 100)] private int id;

    #region Public methods

    #region IInteractive

    public void Interact(Component sender)
    {
        AddToInventory(sender);
    }
    #endregion

    #endregion
    #region Private methods

    private void AddToInventory(Component sender)
    {
        bool senderDoesntHaveInventory = !sender.TryGetComponent(out PlayerInventory inventory);
        if (senderDoesntHaveInventory) return;
        //else...

        inventory.AddKey(id);
        Destroy(gameObject);
    }

    #endregion
}