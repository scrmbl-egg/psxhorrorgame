using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[Header("Keybinds")]
    ////
    //[SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    //[SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;

    [Header("Movement")]
    //
    [SerializeField] private Transform orientation;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField, Range(0f,1f)] private float movementSpeedOnAirMultiplier;
    [SerializeField] private float airDrag = 2f;
    private float _defaultDrag;
    private float _movementSpeedMultiplier = 10;
    private float Horizontal => Input.GetAxisRaw("Horizontal");
    private float Vertical => Input.GetAxisRaw("Vertical");
    private Vector3 _movementDirection;
    private Vector3 _movementDirectionOnSlope;
    private Rigidbody _rigidBody;

    [Header("Ground detection")]
    //
    [SerializeField] private Transform groundCheck;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask canWalkOver;
    private bool IsGrounded => Physics.CheckSphere(position: groundCheck.position,
                                                   radius: groundCheckRadius,
                                                   layerMask: canWalkOver);
    private RaycastHit _slopeHit;
    private bool IsOnSlope
    {
        get
        {
            //raycast to check hitinfo/floor normal
            bool slopeRay = Physics.Raycast(origin: transform.position,
                                            direction: Vector3.down,
                                            hitInfo: out _slopeHit,
                                            maxDistance: (capsuleCollider.height / 2) + .25f,
                                            layerMask: canWalkOver);

            if (!slopeRay) return false; //guard clause
            
            if (_slopeHit.normal != Vector3.up) return true;
            else return false;
        }
    }

    #region MonoBehaviour

    private void Awake()
    {
        //rigidbody setup
        _rigidBody = GetComponent<Rigidbody>();
        _defaultDrag = _rigidBody.drag;
    }

    private void Update()
    {
        InputManagement();
        DragManagement();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnDrawGizmos()
    {
        //ground check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.blue;
        if (IsOnSlope)
        {
            Gizmos.DrawRay(transform.position, _movementDirectionOnSlope.normalized);
        }
        else Gizmos.DrawRay(transform.position, _movementDirection.normalized);
    }

    #endregion

    #region Private methods

    private void InputManagement()
    {
        _movementDirection = orientation.forward * Vertical + orientation.right * Horizontal;
        _movementDirectionOnSlope = Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal);
    }

    private void DragManagement()
    {
        if (IsGrounded) _rigidBody.drag = _defaultDrag;
        else _rigidBody.drag = airDrag;
    }

    private void MovePlayer()
    {
        if (IsGrounded && !IsOnSlope)
        {
            _rigidBody.AddForce(_movementDirection.normalized * movementSpeed * _movementSpeedMultiplier, ForceMode.Acceleration);
        }
        else if (IsGrounded && IsOnSlope)
        {
            _rigidBody.AddForce(_movementDirectionOnSlope.normalized * movementSpeed * _movementSpeedMultiplier, ForceMode.Acceleration);
        }
        else
        {
            _rigidBody.AddForce(_movementDirection.normalized * movementSpeedOnAirMultiplier * movementSpeed * _movementSpeedMultiplier, ForceMode.Acceleration);
        }
    }

    #endregion
}
