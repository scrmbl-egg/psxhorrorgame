using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Weapon selector")]
    //
    public WeaponSelector WeaponSelector;

    [Header("Key List")]
    //
    [SerializeField] private List<int> keys;

    #region Public methods

    public bool HasKeyWithID(int id)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == id) return true;
        }
        //if key is not detected...

        return false;
    }
    
    public void AddKey(int id)
    {
        keys.Add(id);
    }

    public void RemoveKey(int id)
    {
        keys.Remove(id);
    }

    #endregion
}
