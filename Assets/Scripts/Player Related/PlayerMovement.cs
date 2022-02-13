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
    [SerializeField] Transform _orientation;
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField, Range(0f,1f)] float _movementSpeedOnAirMultiplier;
    [SerializeField] float _airDrag = 2f;
    float _defaultDrag;
    float _movementSpeedMultiplier = 10;
    float Horizontal => Input.GetAxisRaw("Horizontal");
    float Vertical => Input.GetAxisRaw("Vertical");
    Vector3 _movementDirection;
    Vector3 _movementDirectionOnSlope;
    Rigidbody _rigidBody;

    [Header("Ground detection")]
    //
    [SerializeField] Transform _groundCheck;
    [SerializeField] CapsuleCollider _capsuleCollider;
    [SerializeField] float _groundCheckRadius;
    [SerializeField] LayerMask _canWalkOver;
    bool IsGrounded => Physics.CheckSphere(position: _groundCheck.position, radius: _groundCheckRadius, layerMask: _canWalkOver);
    RaycastHit _slopeHit;
    bool IsOnSlope
    {
        get
        {
            //raycast to check hitinfo/floor normal
            bool slopeRay = Physics.Raycast(origin: transform.position,
                                            direction: Vector3.down,
                                            hitInfo: out _slopeHit,
                                            maxDistance: (_capsuleCollider.height / 2) + .25f,
                                            layerMask: _canWalkOver);

            if (!slopeRay) return false; //guard clause
            
            if (_slopeHit.normal != Vector3.up) return true;
            else return false;
        }
    }

    #region MonoBehaviour

    void Awake()
    {
        //rigidbody setup
        _rigidBody = GetComponent<Rigidbody>();
        _defaultDrag = _rigidBody.drag;

        //capsule collider setup
        _capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    void Update()
    {
        InputManagement();
        DragManagement();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnDrawGizmos()
    {
        //ground check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);

        Gizmos.color = Color.blue;
        if (IsOnSlope)
        {
            Gizmos.DrawRay(transform.position, _movementDirectionOnSlope.normalized);
        }
        else Gizmos.DrawRay(transform.position, _movementDirection.normalized);
    }

    #endregion

    #region Private methods

    void InputManagement()
    {
        _movementDirection = _orientation.forward * Vertical + _orientation.right * Horizontal;
        _movementDirectionOnSlope = Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal);
    }

    void DragManagement()
    {
        if (IsGrounded) _rigidBody.drag = _defaultDrag;
        else _rigidBody.drag = _airDrag;
    }

    void MovePlayer()
    {
        if (IsGrounded && !IsOnSlope)
        {
            _rigidBody.AddForce(_movementDirection.normalized * _movementSpeed * _movementSpeedMultiplier, ForceMode.Acceleration);
        }
        else if (IsGrounded && IsOnSlope)
        {
            _rigidBody.AddForce(_movementDirectionOnSlope.normalized * _movementSpeed * _movementSpeedMultiplier, ForceMode.Acceleration);
        }
        else
        {
            _rigidBody.AddForce(_movementDirection.normalized * _movementSpeedOnAirMultiplier * _movementSpeed * _movementSpeedMultiplier, ForceMode.Acceleration);
        }
    }

    #endregion
}
