using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Door : MonoBehaviour, IInteractive
{
    [Header("Door Properties")]
    //
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string isLockedMessage;
    public bool IsLocked
    {
        get => isLocked;
        set => isLocked = value;
    }
    public bool IsOpen { get; private set; }
    private Rigidbody _rigidBody;

    [Space(10)]
    [Header("Animation Properties")]
    //
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField, Range(0, MAX_TIME)] private float time;
    [SerializeField] private Vector3 rotationDegrees;
    private Vector3 _startEulerRotation;
    private const float MAX_TIME = 10;

    #region MonoBehaviour

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _startEulerRotation = transform.eulerAngles;
    }

    #endregion

    #region Public methods
    #region IInteractive

    public void Interact()
    {
        if (IsLocked)
        {
            Debug.Log("This door is locked!");
        }
        else
        {
            if (!IsOpen) Open();
            else Close();
        }
    }
    
    #endregion

    public void Open()
    {
        Vector3 targetEulerRotation = _startEulerRotation + rotationDegrees;

        IsOpen = true;
        DOTweenEvents.SimpleRotate(_rigidBody, targetEulerRotation, time, animationCurve);
    }

    public void Close()
    {
        Vector3 targetEulerRotation = _startEulerRotation;

        IsOpen = false;
        DOTweenEvents.SimpleRotate(_rigidBody, targetEulerRotation, time, animationCurve);
    }

    #endregion
}