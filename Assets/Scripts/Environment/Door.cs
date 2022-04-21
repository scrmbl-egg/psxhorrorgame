using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Door : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private Collider pressingArea;

    [Space(10)]
    [Header("Door Properties")]
    //
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool isOpen = false;
    [SerializeField, Range(1, 100)] private int keyId;
    [SerializeField] private string isLockedMessage;
    [SerializeField] private string isUnlockedMessage;
    private Rigidbody _rigidBody;

    public int KeyID => keyId;

    [Space(10)]
    [Header("Animation Properties")]
    //
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField, Range(0, MAX_TIME)] private float time;
    [SerializeField] private Vector3 rotationDegrees;
    private Vector3 _startEulerRotation;
    private const float MAX_TIME = 10;

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool showPressingArea;
    [SerializeField] private Color gizmoColor;

    #region MonoBehaviour

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _startEulerRotation = transform.eulerAngles;
    }

    private void OnDrawGizmos()
    {
        if (showPressingArea) DrawPressingArea();
    }

    #endregion

    #region Public methods
    #region IInteractive

    public void Interact(Component sender)
    {
        if (isLocked)
        {
            TryToUnlock(sender);
        }
        else
        {
            if (!isOpen) Open();
            else Close();
        }
    }
    
    #endregion

    public void SetLockedAndClose()
    {
        isLocked = true;
        if (isOpen) Close();
    }

    public void SetKeyID(int id)
    {
        keyId = id;
    }

    public void Open()
    {
        Vector3 targetEulerRotation = _startEulerRotation + rotationDegrees;

        isOpen = true;
        DOTweenEvents.SimpleRotate(_rigidBody, targetEulerRotation, time, animationCurve);
    }

    public void Close()
    {
        Vector3 targetEulerRotation = _startEulerRotation;

        isOpen = false;
        DOTweenEvents.SimpleRotate(_rigidBody, targetEulerRotation, time, animationCurve);
    }

    #endregion
    #region Private methods

    private void TryToUnlock(Component sender)
    {
        bool senderHasInventory = sender.TryGetComponent(out PlayerInventory inventory);
        bool inventoryHasCorrectKey = inventory.HasKeyWithID(keyId);

        if (senderHasInventory && inventoryHasCorrectKey)
        {
            inventory.RemoveKey(keyId);
            UnlockDoor();
        }
        else ShowLockedMessage();
    }

    private void UnlockDoor()
    {
        isLocked = false;
        Debug.Log(isUnlockedMessage);

        //TODO: Display unlock message on screen.
        //TODO: play unlock sound
    }

    private void ShowLockedMessage()
    {
        Debug.Log(isLockedMessage);

        //TODO: Display lock message on screen.
        //TODO: play lock sound
    }

    private void DrawPressingArea()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawCube(pressingArea.bounds.center, pressingArea.bounds.size);
    }

    #endregion
}