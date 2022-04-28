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
    [Header("Noise Properties")]
    //
    [SerializeField, Range(0, 1)] private float perlinDirectionX;
    [SerializeField, Range(0, 1)] private float perlinDirectionY;
    Vector2 currentPerlinPosition;

    #region MonoBehaviour

    private void Update()
    {
        ManageRotation();
    }

    #endregion

    #region Public methods

    public void ShakeCamera(in Vector3 direction, in float shakeSpeed, in float recoverSpeed)
    {
        _shakeSpeed = shakeSpeed;
        _recoverSpeed = recoverSpeed;

        currentPerlinPosition.x += perlinDirectionX;
        currentPerlinPosition.y += perlinDirectionY; 

        const float MIN_VALUE = -1;
        const float MAX_VALUE = 1;
        const float NOISE_MULTIPLIER = MAX_VALUE - MIN_VALUE;

        float noise = Mathf.PerlinNoise(currentPerlinPosition.x, currentPerlinPosition.y);
        float multipliedNoise = noise * NOISE_MULTIPLIER;
        float randomIntensity = MIN_VALUE + multipliedNoise; //noise range from -1 to 1

        float x = -direction.x * multipliedNoise;  //x axis only rotates upward
        float y = direction.y * randomIntensity;   //y axis rotates left or right
        float z = direction.z * randomIntensity;   //z axis tilts clockwise or counter-clockwise
        Vector3 targetVector = new Vector3(x, y, z);

        _desiredRotation += targetVector;
    }

    #endregion
    #region Private methods

    private void ManageRotation()
    {
        float time1 = _recoverSpeed * Time.deltaTime;
        Vector3 lerpTowardsZero = 
            Vector3.Lerp(_desiredRotation, Vector3.zero, time1);

        _desiredRotation = lerpTowardsZero;

        float time2 = _shakeSpeed * Time.deltaTime;
        Vector3 lerpTowardsDesiredRotation = 
            Vector3.Lerp(_currentRotation, _desiredRotation, time2);

        _currentRotation = lerpTowardsDesiredRotation;
        cam.localRotation = Quaternion.Euler(_currentRotation);
    }

    #endregion
}