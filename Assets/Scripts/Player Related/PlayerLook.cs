using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    //input
    PlayerInputActions _playerInputActions;

    [Header("Camera")]
    //
    [SerializeField] private Transform camHolder;
    [SerializeField] private Transform orientation;
    public Camera Cam;
    
    [Header("Rotation")]
    //
    [SerializeField] private GameObject playerViewport;
    private float _pitch;
    private float _yaw;

    [Header("Field of view")]
    //
    [SerializeField] private float additionalFovWhenRunning;
    private float defaultFov;

    #region MonoBehaviour

    private void Awake()
    {
        //input
        _playerInputActions = new PlayerInputActions();
        
        //cursor setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //fov
        defaultFov = Cam.fieldOfView;
    }

    private void OnEnable()
    {
        _playerInputActions.PlayerThing.Look.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Look.Disable();
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
        Vector2 input = _playerInputActions.PlayerThing.Look.ReadValue<Vector2>();

        //mouse look
        _pitch -= input.y;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f); //clamp pitch up and down

        _yaw += input.x;
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
