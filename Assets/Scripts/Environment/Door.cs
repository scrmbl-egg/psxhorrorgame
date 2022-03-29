using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Door : MonoBehaviour, IInteractive
{
    [Header("Door Properties")]
    //
    [SerializeField] bool isLocked = false;
    [SerializeField] string isLockedMessage;
    public bool Locked
    {
        get => isLocked;
        set => isLocked = value;
    }

    [Space(10)]
    [Header("Animation Properties")]
    //
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField, Range(0, _maxTime)] float time;
    [SerializeField] Vector3 rotationDegrees;
    Vector3 _startEulerRotation;
    const float _maxTime = 100;

    //dependencies
    Rigidbody _rigidBody;
    
    public bool IsOpen { get; private set; }

    #region MonoBehaviour

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _startEulerRotation = transform.eulerAngles;
    }

    void OnDrawGizmos()
    {
    }

    #endregion

    #region Public methods
    #region IInteractive

    public void Interact()
    {
        if (Locked)
        {
            Debug.Log("This door is locked!");
        }
        else
        {
            if (!IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }
    
    #endregion

    public void Open()
    {
        Vector3 targetEulerRotation = _startEulerRotation + rotationDegrees;

        IsOpen = true;
        DOTweenEvents.SimpleRotate(_rigidBody, targetEulerRotation, time * Time.deltaTime, animationCurve);
    }

    public void Close()
    {
        Vector3 targetEulerRotation = _startEulerRotation;

        IsOpen = false;
        DOTweenEvents.SimpleRotate(_rigidBody, targetEulerRotation, time * Time.deltaTime, animationCurve);
    }

    #endregion
}