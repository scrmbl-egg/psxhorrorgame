using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    #region MonoBehaviour
    
    private void Update()
    {
        ManagePosition();
        ManageRotation();
    }


    #endregion
    #region Private methods
    private void ManagePosition()
    {
        transform.position = cameraPosition.position;
    }

    private void ManageRotation()
    {
        float clampedX = Mathf.Clamp(transform.eulerAngles.x, -90, 90);
        float y = transform.eulerAngles.y;
        float z = transform.eulerAngles.z;
        Vector3 clampedRotation = new Vector3(clampedX, y, z);
        transform.rotation = Quaternion.Euler(clampedRotation);
    }

    #endregion
}
