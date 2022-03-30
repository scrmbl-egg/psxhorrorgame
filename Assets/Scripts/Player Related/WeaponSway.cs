using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Dependencies")]
    //
    [SerializeField] private PlayerCamera playerCamera;

    [Space(10)]
    [Header("Settings")]
    //
    [SerializeField] private float smoothTime;
    [SerializeField] private float swayMultiplier;

    #region MonoBehaviour

    private void Update()
    {
        SwayWeapon();
    }

    #endregion

    #region Private methods

    private void SwayWeapon()
    {
        //calculate x and y rotation based on player input
        float playerMouseYInput = -playerCamera.MouseY * playerCamera.InvertY;
        float playerMouseXInput = playerCamera.MouseX * playerCamera.InvertX;


        Quaternion xRotation = Quaternion.AngleAxis(playerMouseYInput * swayMultiplier, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(playerMouseXInput * swayMultiplier, Vector3.up);
        Quaternion targetRotation = xRotation * yRotation;

        //interpolate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothTime * Time.deltaTime);
    }

    #endregion

}
