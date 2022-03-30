using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    #region MonoBehaviour
    
    private void Update()
    {
        transform.position = cameraPosition.position;
    }

    #endregion
}
