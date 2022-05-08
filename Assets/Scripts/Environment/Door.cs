using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Door : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private Collider interactionArea;
    private static Dialogue _dialogue;

    [Space(10)]
    [Header("Door Properties")]
    //
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool isOpen = false;
    [SerializeField, Range(1, 100)] private int keyId;
    [SerializeField, TextArea(1, 3)] private string isLockedMessage;
    [SerializeField, TextArea(1, 3)] private string isUnlockedMessage;
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
    [SerializeField] private bool showInteractionArea;
    [SerializeField] private Color gizmoColor;

    #region MonoBehaviour

    private void Awake()
    {
        if (_dialogue == null) _dialogue = FindObjectOfType<Dialogue>();
        
        _rigidBody = GetComponent<Rigidbody>();

        _startEulerRotation = transform.eulerAngles;
    }

    private void OnDrawGizmos()
    {
        if (showInteractionArea) DrawInteractionArea();
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
        isOpen = true;

        Vector3 targetEulerRotation = _startEulerRotation + rotationDegrees;
        DOTweenEvents.SimpleRotate(rigidbody: _rigidBody,
                                   rotationDegrees: targetEulerRotation,
                                   time: time,
                                   animationCurve: animationCurve);
    }

    public void Close()
    {
        isOpen = false;

        Vector3 targetEulerRotation = _startEulerRotation;
        DOTweenEvents.SimpleRotate(rigidbody: _rigidBody,
                                   rotationDegrees: targetEulerRotation,
                                   time: time,
                                   animationCurve: animationCurve);
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
        else
        {
            ShowLockedMessage();
        }
    }

    private void UnlockDoor()
    {
        isLocked = false;

        _dialogue.PrintMessage(isUnlockedMessage);
        //TODO: Display unlock message on screen.
        //TODO: play unlock sound
    }

    private void ShowLockedMessage()
    {
        _dialogue.PrintMessage(isLockedMessage);

        //TODO: Display lock message on screen.
        //TODO: play lock sound
    }

    private void DrawInteractionArea()
    {
        Vector3 origin = interactionArea.bounds.center;
        Vector3 size = interactionArea.bounds.size;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(origin, size);
    }

    #endregion
}