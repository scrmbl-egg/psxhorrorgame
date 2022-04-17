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
        get => keys;
        set => keys = value;
    }

    #region Public methods

    public void UseKeys(int keyId)
    {
        for (int i = 0; i < Keys.Count; i++)
        {
            if (i != keyId) continue;
            
            //else...
            Keys.Remove(i);
        }
    }
    
    public void AddKey(int keyId)
    {
        Keys.Add(keyId);
    }

    #endregion
}
