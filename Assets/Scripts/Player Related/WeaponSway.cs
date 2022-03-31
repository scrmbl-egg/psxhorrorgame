using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Settings")]
    //
    [SerializeField] private float smooth;
    [SerializeField] private float intensity;
    Quaternion _originalLocalRotation;
    const float SMOOTH_MULTIPLIER = 0.01f;

    #region MonoBehaviour

    private void Awake()
    {
        _originalLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        Sway();
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

        float xInput = Input.GetAxisRaw("Mouse X");
        float yInput = Input.GetAxisRaw("Mouse Y") * -1;

        float clampedXInput = Mathf.Clamp(xInput, -1, 1);
        float clampedYInput = Mathf.Clamp(yInput, -1, 1);
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

}
