using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class WeaponSway : MonoBehaviour
{
    //input
    PlayerInputActions _playerInputActions;

    [Header("Settings")]
    //
    [SerializeField] private float smooth;
    [SerializeField] private float intensity;
    Quaternion _originalLocalRotation;
    const float SMOOTH_MULTIPLIER = 0.01f;

    [Header("Bobbing")]
    //[SerializeField] private float _resetRecoverT = .25f;
    //[SerializeField] private float bobScale = .1f;
    //[SerializeField] private float bobSpeed = 3f;

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float lastPosition;
    [SerializeField] private float animationTime;
    private Tween BobbingTween;

    private Vector3 _restPos;

    #region MonoBehaviour

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _originalLocalRotation = transform.localRotation;

        _restPos = transform.localPosition;

        BobbingTween = transform.DOLocalMoveY(lastPosition, animationTime).SetLoops(-1).SetEase(animationCurve);

        //if (dEventType == DOTweenEventType.Bobbing)
        //{
        //    _bobbingTween = DOTweenModulePhysics.DOMoveY(_rb ,_lastPosition, _animTime).SetLoops(-1).SetEase(_animCurve);
        //}
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

        float clampedXInput = Mathf.Clamp(input.x, -1, 1);
        float clampedYInput = Mathf.Clamp(input.y, -1, 1) * -1;
        float clampedSmooth = Mathf.Clamp01(smooth * SMOOTH_MULTIPLIER);

        //calculate rotations
        Quaternion xRotation = Quaternion.AngleAxis(clampedYInput * intensity, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(clampedXInput * intensity, Vector3.up);
        Quaternion targetRotation = _originalLocalRotation * xRotation * yRotation;

        //interpolate
        transform.localRotation = Quaternion.Slerp(a: transform.localRotation,
                                                   b: targetRotation,
                                                   t: clampedSmooth);
    }
    #endregion

    #region Bobbing
    private void Bobbing()
    {
        Vector2 input = _playerInputActions.PlayerThing.Move.ReadValue<Vector2>();

        if (input.magnitude > Mathf.Epsilon)
        {
            DOTween.Play(BobbingTween);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        //if (input.magnitude > Mathf.Epsilon)
        //{
        //    float scale = 2 / (3 - Mathf.Cos(2 * Time.time));
        //    transform.localPosition = new Vector3(
        //                transform.localPosition.x + bobScale * (scale * Mathf.Cos(Time.time * bobScale) * Time.deltaTime),
        //                transform.localPosition.y + bobScale * (scale * Mathf.Sin(2 * Time.time * bobSpeed) / 2 * Time.deltaTime),
        //                transform.localPosition.z
        //            );
        //}else
        //{
        //    transform.DOLocalMove(_restPos, _resetRecoverT);
        //}
        
    }
    #endregion
}