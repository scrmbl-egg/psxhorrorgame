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
    [SerializeField, Range(1, 100)] private int keyId;
    private Rigidbody _rigidBody;
    public bool IsLocked
    {
        get => isLocked;
        private set => isLocked = value;
    }
    public bool IsOpen { get; private set; }

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

    public void Interact(Component sender)
    {
        if (IsLocked)
        {
            TryToUnlock(sender);
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
    #region Private methods

    private void TryToUnlock(Component sender)
    {
        bool senderDoesntHaveInventory = !sender.TryGetComponent(out PlayerInventory inventory);
        if (senderDoesntHaveInventory && inventory.HasKeyWithID(keyId))
        {
            inventory.RemoveKey(keyId);
            UnlockDoor();
        }
        else ShowLockedMessage();
    }

    private void UnlockDoor()
    {
        isLocked = false;
        
        Debug.Log("door is now unlocked!");

        //TODO: Unlock sound
    }

    private void ShowLockedMessage()
    {
        Debug.Log(isLockedMessage);
    }

    #endregion
}