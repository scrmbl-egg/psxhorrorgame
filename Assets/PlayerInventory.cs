using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Key List")]
    //
    [SerializeField] private List<int> keys;
    public List<int> Keys
    {
        set => keys = value;
    }

    #region Public methods

    public bool HasKeyWithID(int id)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (i == id) return true;
        }

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
