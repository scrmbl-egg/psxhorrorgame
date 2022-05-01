using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [Header("Dependencies")]
    //
    [SerializeField] private Transform cam;
    private float _shakeSpeed;
    private float _recoverSpeed;
    private Vector3 _desiredRotation;
    private Vector3 _currentRotation;

    [Space(10)]
    [Header("Settings")]
    //
    [SerializeField, Min(float.Epsilon)] private float defaultShakeSpeed;
    [SerializeField, Min(float.Epsilon)] private float defaultRecoverSpeed;
    [SerializeField] private Vector3 defaultAnglesRange;

    [Space(10)]
    [Header("Noise Properties")]
    //
    [SerializeField, Range(0, 1)] private float perlinDirectionX;
    [SerializeField, Range(0, 1)] private float perlinDirectionY;
    private Vector2 _currentPerlinPosition;

    #region MonoBehaviour

    private void Update()
    {
        ManageRotation();
    }

    #endregion

    #region Public methods

    public void ShakeCamera(in Vector3 angles, in float shakeSpeed, in float recoverSpeed)
    {
        _shakeSpeed = shakeSpeed;
        _recoverSpeed = recoverSpeed;

        //new perlin noise value
        Vector2 step = new Vector2(perlinDirectionX, perlinDirectionY);
        _currentPerlinPosition += step;
        
        //make perlin noise range from -1 to 1
        const float MIN_VALUE = -1;
        const float MAX_VALUE = 1;
        const float NOISE_MULTIPLIER = MAX_VALUE - MIN_VALUE;

        float noise = Mathf.PerlinNoise(_currentPerlinPosition.x, _currentPerlinPosition.y);
        float multipliedNoise = noise * NOISE_MULTIPLIER;
        float randomIntensity = MIN_VALUE + multipliedNoise;

        //generate vector
        float x = -angles.x * multipliedNoise;  //x axis only rotates upward
        float y = angles.y * randomIntensity;   //y axis rotates left or right
        float z = angles.z * randomIntensity;   //z axis tilts clockwise or counter-clockwise
        Vector3 targetVector = new Vector3(x, y, z);

        _desiredRotation += targetVector;
    }

    public void ShakeCamera()
    {
        _shakeSpeed = defaultShakeSpeed;
        _recoverSpeed = defaultRecoverSpeed;

        //setup for random values between -1 to 1
        const float MIN_VALUE = -1;
        const float MAX_VALUE = 1;
        const float MULTIPLIER = MAX_VALUE - MIN_VALUE;

        //generate vector
        float x = defaultAnglesRange.x * (MIN_VALUE + Random.value * MULTIPLIER);
        float y = defaultAnglesRange.y * (MIN_VALUE + Random.value * MULTIPLIER);
        float z = defaultAnglesRange.z * (MIN_VALUE + Random.value * MULTIPLIER);
        Vector3 targetVector = new Vector3(x, y, z);

        _desiredRotation += targetVector;
    }

    #endregion
    #region Private methods

    private void ManageRotation()
    {
        float time1 = _recoverSpeed * Time.deltaTime;
        float time2 = _shakeSpeed * Time.deltaTime;
        Vector3 lerpTowardsZero = Vector3.Lerp(_desiredRotation, Vector3.zero, time1);
        Vector3 lerpTowardsDesiredRotation = Vector3.Lerp(_currentRotation, _desiredRotation, time2);

        _desiredRotation = lerpTowardsZero;
        _currentRotation = lerpTowardsDesiredRotation;
        cam.localRotation = Quaternion.Euler(_currentRotation);
    }

    #endregion
}