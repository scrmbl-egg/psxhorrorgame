using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerLook : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header("Camera")]
    //
    [SerializeField] private Transform camHolder;
    [SerializeField] private Transform orientation;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private Camera weaponCamera;
    
    [Header("Rotation")]
    //
    [SerializeField] private GameObject playerViewport;
    private float _pitch;
    private float _yaw;

    [Header("Field of view")]
    //
    [SerializeField] private PlayerMovement player;
    [SerializeField, Min(float.Epsilon)] private float additionalFovWhenRunning;
    [SerializeField, Min(float.Epsilon)] private float reducedFovWhenAiming;
    [Space(2)]
    [SerializeField, Min(float.Epsilon)] private float fovLerpingTime;
    private float _defaultFov;

    public bool IsAiming { get; set; }

    #region MonoBehaviour

    private void Awake()
    {
        //input
        _playerInputActions = new PlayerInputActions();
        
        //cursor setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //fov
        _defaultFov = worldCamera.fieldOfView;
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
        ManageInput();
        RotateCamera();
        ManageFov();
        RotateViewport();
    }

    #endregion

    #region Private methods

    private void ManageInput()
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

    private void ManageFov()
    {
        float runningFov = _defaultFov + additionalFovWhenRunning;
        float aimingFov = _defaultFov - reducedFovWhenAiming;
        float t = Time.deltaTime * fovLerpingTime;
        
        float lerpTowardsRunningFov = Mathf.Lerp(worldCamera.fieldOfView, runningFov, t);
        float lerpTowardsAimingFov = Mathf.Lerp(worldCamera.fieldOfView, aimingFov, t);
        float lerpTowardsDefaultFov = Mathf.Lerp(worldCamera.fieldOfView, _defaultFov, t);

        //if (player.IsRunning) worldCamera.fieldOfView = lerpTowardsRunningFov;
        //else if (IsAiming) worldCamera.fieldOfView = lerpTowardsAimingFov;
        //else worldCamera.fieldOfView = lerpTowardsDefaultFov;

        if (player.IsRunning)
        {
            worldCamera.fieldOfView = lerpTowardsRunningFov;
            weaponCamera.fieldOfView = lerpTowardsRunningFov;
        }
        else if (IsAiming)
        {
            worldCamera.fieldOfView = lerpTowardsAimingFov;
            weaponCamera.fieldOfView = lerpTowardsAimingFov;
        }
        else
        {
            worldCamera.fieldOfView = lerpTowardsDefaultFov;
            weaponCamera.fieldOfView = lerpTowardsDefaultFov;
        }
    }

    private void RotateViewport()
    {
        playerViewport.transform.rotation = orientation.rotation;
    }

    #endregion
}