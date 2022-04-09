using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //input
    PlayerInputActions _playerInputActions;

    [Header("Movement")]
    //
    [SerializeField] private Transform orientation;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField, Range(0f,1f)] private float movementSpeedOnAirMultiplier;
    [SerializeField] private float airDrag = 2f;
    private float _defaultDrag;
    private float _movementSpeedMultiplier = 10;
    //private float Horizontal => Input.GetAxisRaw("Horizontal");
    //private float Vertical => Input.GetAxisRaw("Vertical");
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
        //input
        _playerInputActions = new PlayerInputActions();

        //rigidbody setup
        _rigidBody = GetComponent<Rigidbody>();
        _defaultDrag = _rigidBody.drag;
    }

    private void OnEnable()
    {
        _playerInputActions.PlayerThing.Move.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Move.Disable();
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
        Vector2 input = _playerInputActions.PlayerThing.Move.ReadValue<Vector2>();

        _movementDirection = orientation.forward * input.y + orientation.right * input.x;
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
