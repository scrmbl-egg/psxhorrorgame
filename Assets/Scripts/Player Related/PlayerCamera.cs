using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Cámara")]
    //
    [SerializeField] Transform _camHolder;
    [SerializeField] Transform _orientation;
    
    [Header("Rotación")]
    //
    [SerializeField] GameObject _playerViewport;
    [SerializeField] float _sensitivity = 50;
    [SerializeField] bool _invertX;
    [SerializeField] bool _invertY;
    float _sensitivityMultiplier = .01f;
    float MouseX => Input.GetAxisRaw("Mouse X");
    float MouseY => Input.GetAxisRaw("Mouse Y");
    int InvertX
    {
        get
        {
            if (_invertX) return -1;
            else return 1;
        }
    }
    int InvertY
    {
        get
        {
            if (_invertY) return -1;
            else return 1;
        }
    }
    float _pitch;
    float _yaw;

    [Header("Campo de visión")]
    //
    [SerializeField] float _defaultFov;

    #region MonoBehaviour

    void Awake()
    {
        //cursor setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        InputManagement();
        RotateCamera();
    }

    #endregion

    #region Private methods

    void InputManagement()
    {
        _pitch -= MouseY * InvertY * _sensitivity * _sensitivityMultiplier;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);

        _yaw += MouseX * InvertX * _sensitivity * _sensitivityMultiplier;
    }

    void RotateCamera()
    {
        //rotate camera
        _camHolder.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
        _orientation.rotation = Quaternion.Euler(0, _yaw, 0);

        //rotate capsule
        _playerViewport.transform.rotation = _orientation.rotation;
    }

    #endregion
}
