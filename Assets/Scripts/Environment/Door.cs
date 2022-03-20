using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour, IInteractive
{
    public bool DoorIsClosed;

    #region MonoBehaviour

    void Awake()
    {

    }

    #endregion

    #region Public methods
    #region IInteractive

    public void Interact()
    {
        if (DoorIsClosed)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
    
    #endregion

    public void Open()
    {
        Debug.Log("Opened door");
        DoorIsClosed = false;
    }

    public void Close()
    {
        Debug.Log("Closed the door");
        DoorIsClosed = true;
    }

    #endregion
}
