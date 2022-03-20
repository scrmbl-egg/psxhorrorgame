using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera")]
    //
    [SerializeField] Transform camHolder;
    [SerializeField] Transform orientation;
    public Camera Camera;
    
    [Header("Rotation")]
    //
    [SerializeField] GameObject playerViewport;
    [SerializeField] float sensitivity = 50;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    float _sensitivityMultiplier = .01f;
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
    float _pitch;
    float _yaw;

    [Header("Field of view")]
    //
    [SerializeField] float additionalFovWhenRunning;
    float defaultFov;

    #region MonoBehaviour

    void Awake()
    {
        //cursor setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //fov
        defaultFov = Camera.fieldOfView;
    }

    void Update()
    {
        InputManagement();
        RotateCamera();
        RotateViewport();
    }

    #endregion

    #region Private methods

    void InputManagement()
    {
        //mouse look
        _pitch -= MouseY * InvertY * sensitivity * _sensitivityMultiplier;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f); //clamp pitch up and down

        _yaw += MouseX * InvertX * sensitivity * _sensitivityMultiplier;
    }

    void RotateCamera()
    {
        camHolder.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
        orientation.rotation = Quaternion.Euler(0, _yaw, 0);
    }

    void RotateViewport()
    {
        playerViewport.transform.rotation = orientation.rotation;
    }

    #endregion
}
