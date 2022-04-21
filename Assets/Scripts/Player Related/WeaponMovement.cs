using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header("Dependencies")]
    //
    [SerializeField] private PlayerMovement player;

    [Space(10)]
    [Header("Sway Settings")]
    //
    [SerializeField] private float smooth;
    [SerializeField] private float intensity;
    private Quaternion _originalLocalRotation;

    [Space(10)]
    [Header("Bobbing Settings")]
    //
    [SerializeField] private AnimationCurve bobX;
    [SerializeField, Range(0, 0.2f)] private float xRange;
    [SerializeField] private AnimationCurve bobY;
    [SerializeField, Range(0, 0.2f)] private float yRange;
    [SerializeField] private AnimationCurve bobZ;
    [SerializeField, Range(0, 0.2f)] private float zRange;
    [Space(2)]
    [SerializeField, Min(float.Epsilon)] private float duration;
    [SerializeField, Range(1, 5)] private float bobSpeedWhenRunning;
    [SerializeField] private float lerpTime;
    private float _currentBobSpeed = 1;
    private float _curveEvaluation;

    #region MonoBehaviour

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _originalLocalRotation = transform.localRotation;
    }

    private void OnEnable()
    {
        _playerInputActions.PlayerThing.Look.Enable();
        _playerInputActions.PlayerThing.Move.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.PlayerThing.Look.Disable();
        _playerInputActions.PlayerThing.Move.Disable();
    }

    private void Update()
    {
        Sway();
        Bobbing();
    }

    #endregion

    #region Private methods

    private void Sway()
    {
        ///setup explanation:
        /// 
        ///The sway motion isn't framerate independent, but using Time.deltaTime causes an
        ///uncomfortable snap of the weapon when the framerate lowers. This method has been
        ///tested with 30, 60, and unlimited (around 300-400) target fps, and it successfully
        ///removes the random snapping completely, while making the intensity difference
        ///between frame rates completely trivial.

        Vector2 input = _playerInputActions.PlayerThing.Look.ReadValue<Vector2>();
        const float SMOOTH_MULTIPLIER = 0.01f;

        float clampedXInput = Mathf.Clamp(input.x, -1, 1);
        float clampedYInput = Mathf.Clamp(input.y, -1, 1) * -1;
        float clampedSmooth = Mathf.Clamp01(smooth * SMOOTH_MULTIPLIER);

        //calculate rotations
        Quaternion xRotation = Quaternion.AngleAxis(clampedYInput * intensity, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(clampedXInput * intensity, Vector3.up);
        Quaternion targetRotation = _originalLocalRotation * xRotation * yRotation;

        //interpolate
        transform.localRotation = 
            Quaternion.Slerp(a: transform.localRotation,
                             b: targetRotation,
                             t: clampedSmooth);
    }

    private void Bobbing()
    {
        //movement intensity
        Vector2 input = _playerInputActions.PlayerThing.Move.ReadValue<Vector2>();
        float movementIntensity = Mathf.Clamp01(input.magnitude);

        //curves and bob speed
        const float CURVE_TIME = 1;

        float t = Time.deltaTime * lerpTime;
        float lerpTowardsRunningBob = 
            Mathf.Lerp(a: _currentBobSpeed,
                       b: bobSpeedWhenRunning,
                       t: t);
        float lerpTowardsDefaultBob = 
            Mathf.Lerp(a: _currentBobSpeed,
                       b: 1,
                       t: t);

        if (player.IsRunning) _currentBobSpeed = lerpTowardsRunningBob;
        else _currentBobSpeed = lerpTowardsDefaultBob;

        _curveEvaluation += (Time.deltaTime * _currentBobSpeed) / (duration);
        if (_curveEvaluation >= CURVE_TIME) _curveEvaluation = 0;

        //animation
        Vector3 bobEvaluation = 
            new Vector3(x: bobX.Evaluate(_curveEvaluation) * xRange,
                        y: bobY.Evaluate(_curveEvaluation) * yRange,
                        z: bobZ.Evaluate(_curveEvaluation) * zRange);

        Vector3 newPosition = bobEvaluation * movementIntensity;

        transform.localPosition = 
            Vector3.Slerp(a: transform.localPosition,
                          b: newPosition,
                          t: t);
    }

    #endregion
}