using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Dependencies")]
    //
    [SerializeField] PlayerCamera playerCamera;

    [Space(10)]
    [Header("Settings")]
    //
    [SerializeField] float smoothTime;
    [SerializeField] float swayMultiplier;

    #region MonoBehaviour

    void Update()
    {
        SwayWeapon();
    }

    #endregion

    #region Private methods

    void SwayWeapon()
    {
        //calculate x and y rotation based on player input
        Quaternion xRotation = Quaternion.AngleAxis(-playerCamera.MouseY * playerCamera.InvertY, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(playerCamera.MouseX * playerCamera.InvertX, Vector3.up);

        Quaternion targetRotation = xRotation * yRotation;

        //interpolate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothTime * Time.deltaTime);
    }

    #endregion

}
