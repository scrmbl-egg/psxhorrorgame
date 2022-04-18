using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header("Movement")]
    //
    [SerializeField] private Transform orientation;
    [Space(2)]
    [SerializeField] private float defaultMovementSpeed = 2f;
    [Space(2)]
    [SerializeField, Range(1, 5)] private float movementWhenRunningMultiplier;
    [SerializeField] private float movementLerpingTime;
    [SerializeField, Range(0, 1)] private float movementSpeedOnAirMultiplier;
    [SerializeField] private float airDrag = 2f;
    private float _currentMovementSpeed;
    private float _defaultDrag;
    private const float MOVEMENT_SPEED_MULTIPLIER = 10;
    private Vector3 _movementDirection;
    private Vector3 _movementDirectionOnSlope;
    private Rigidbody _rigidBody;
    private bool _runningIsPressed;
    public bool IsRunning { get; private set; }

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
        _playerInputActions.PlayerThing.Run.Enable();

        _playerInputActions.PlayerThing.Run.started += RunDetection;
        _playerInputActions.PlayerThing.Run.performed += RunDetection;
        _playerInputActions.PlayerThing.Run.canceled += RunDetection;
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Move.Disable();
        _playerInputActions.PlayerThing.Run.Disable();

        _playerInputActions.PlayerThing.Run.started -= RunDetection;
        _playerInputActions.PlayerThing.Run.performed += RunDetection;
        _playerInputActions.PlayerThing.Run.canceled -= RunDetection;
    }

    private void Update()
    {
        ManageMovement();
        ManageRunning();
        ManageSpeed();
        ManageDrag();
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

    #region Input

    private void RunDetection(InputAction.CallbackContext ctx)
    {
        bool buttonIsPressed = ctx.ReadValueAsButton();

        if (buttonIsPressed) _runningIsPressed = true;
        else _runningIsPressed = false;
    }

    #endregion

    private void ManageMovement()
    {
        Vector2 input = _playerInputActions.PlayerThing.Move.ReadValue<Vector2>();

        _movementDirection = orientation.forward * input.y + orientation.right * input.x;
        _movementDirectionOnSlope = Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal);
    }

    private void ManageRunning()
    {
        bool playerIsMoving = _movementDirection != Vector3.zero;
        bool playerCanRun = _runningIsPressed && playerIsMoving;

        if (playerCanRun) IsRunning = true;
        else IsRunning = false;
    }

    private void ManageSpeed()
    {
        float runningSpeed = defaultMovementSpeed * movementWhenRunningMultiplier;
        float t = Time.deltaTime * movementLerpingTime;

        float lerpTowardsRunningSpeed = Mathf.Lerp(_currentMovementSpeed, runningSpeed, t);
        float lerpTowardsDefaultSpeed = Mathf.Lerp(_currentMovementSpeed, defaultMovementSpeed, t);

        if (IsRunning) _currentMovementSpeed = lerpTowardsRunningSpeed;
        else _currentMovementSpeed = lerpTowardsDefaultSpeed;
    }

    private void ManageDrag()
    {
        if (IsGrounded) _rigidBody.drag = _defaultDrag;
        else _rigidBody.drag = airDrag;
    }

    private void MovePlayer()
    {
        if (IsGrounded && !IsOnSlope)
        {
            Vector3 movement = _movementDirection * _currentMovementSpeed * MOVEMENT_SPEED_MULTIPLIER;
            _rigidBody.AddForce(movement, ForceMode.Acceleration);
        }
        else if (IsGrounded && IsOnSlope)
        {
            Vector3 movement = _movementDirectionOnSlope * _currentMovementSpeed * MOVEMENT_SPEED_MULTIPLIER;
            _rigidBody.AddForce(movement, ForceMode.Acceleration);
        }
        else //player is in the air
        {
            Vector3 movement = _movementDirection * movementSpeedOnAirMultiplier * _currentMovementSpeed * MOVEMENT_SPEED_MULTIPLIER;
            _rigidBody.AddForce(movement, ForceMode.Acceleration);
        }
    }

    #endregion
}