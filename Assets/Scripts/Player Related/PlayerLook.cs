using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Camera")]
    //
    [SerializeField] private Transform camHolder;
    [SerializeField] private Transform orientation;
    public Camera Cam;
    
    [Header("Rotation")]
    //
    [SerializeField] private GameObject playerViewport;
    [SerializeField] private float sensitivity = 50;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;
    private float _sensitivityMultiplier = .01f;
    public float MouseX => Input.GetAxisRaw("Mouse X");
    public float MouseY => Input.GetAxisRaw("Mouse Y");
    public int InvertX
    {
        get
        {
            if (invertX) return -1;
            else return 1;
        }
    }
    public int InvertY
    {
        get
        {
            if (invertY) return -1;
            else return 1;
        }
    }
    private float _pitch;
    private float _yaw;

    [Header("Field of view")]
    //
    [SerializeField] private float additionalFovWhenRunning;
    private float defaultFov;

    #region MonoBehaviour

    private void Awake()
    {
        //cursor setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //fov
        defaultFov = Cam.fieldOfView;
    }

    private void Update()
    {
        InputManagement();
        RotateCamera();
        RotateViewport();
    }

    #endregion

    #region Private methods

    private void InputManagement()
    {
        //mouse look
        _pitch -= MouseY * InvertY * sensitivity * _sensitivityMultiplier;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f); //clamp pitch up and down

        _yaw += MouseX * InvertX * sensitivity * _sensitivityMultiplier;
    }

    private void RotateCamera()
    {
        camHolder.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
        orientation.rotation = Quaternion.Euler(0, _yaw, 0);
    }

    private void RotateViewport()
    {
        playerViewport.transform.rotation = orientation.rotation;
    }

    #endregion
}
