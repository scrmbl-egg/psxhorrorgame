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

    public void Interact()
    {
        AddKeyToPlayerInventory();
    }
    #endregion

    #endregion
    #region Private methods

    private void AddKeyToPlayerInventory()
    {
        PlayerInventory player = FindObjectOfType<PlayerInventory>();
        if (player == null) return;

        player.AddKey(id);
        Destroy(gameObject);
    }

    #endregion
}