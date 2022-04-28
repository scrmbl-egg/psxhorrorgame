using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Dependencies")]
    //
    [SerializeField] private Transform cam;

    [Space(10)]
    [Header("Testing Properties")]
    //
    [SerializeField] private float recoilSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private Vector3 recoilRotation;
    private Vector3 _desiredRotation;
    private Vector3 _currentRotation;

    [Space(10)]
    [Header("Noise Properties")]
    //
    [SerializeField] private int perlinNoiseJump;
    Vector2 _perlinPosition;

    #region MonoBehaviour

    private void Update()
    {
        ManageRotation();

        if (Input.GetKeyDown(KeyCode.Mouse0)) ApplyRecoil();
    }

    #endregion

    #region Public methods

    [ContextMenu("Recoil")]
    public void ApplyRecoil()
    {
        ///TODO: Use perlin noise in a more effective way.
        ///I don't know if the method below is a good one.
        
        _perlinPosition.x += Time.time;
        _perlinPosition.y = 0;

        const float MIN_VALUE = -1;
        const float MAX_VALUE = 1;
        const float NOISE_MULTIPLIER = MAX_VALUE - MIN_VALUE;

        float noise = Mathf.PerlinNoise(_perlinPosition.x, _perlinPosition.y);
        float multipliedNoise = noise * NOISE_MULTIPLIER;
        float randomIntensity = MIN_VALUE + multipliedNoise; //noise range from -1 to 1

        float x = -recoilRotation.x * multipliedNoise;  //x axis only rotates upward
        float y = recoilRotation.y * randomIntensity;   //y axis rotates left or right
        float z = recoilRotation.z * randomIntensity;   //z axis tilts clockwise or counter-clockwise
        Vector3 targetVector = new Vector3(x, y, z);

        _desiredRotation += targetVector;
    }

    #endregion
    #region Private methods

    private void ManageRotation()
    {
        float time1 = returnSpeed * Time.deltaTime;
        Vector3 lerpTowardsZero = Vector3.Lerp(_desiredRotation, Vector3.zero, time1);

        _desiredRotation = lerpTowardsZero;

        float time2 = recoilSpeed * Time.deltaTime;
        Vector3 lerpTowardsDesiredRotation = Vector3.Lerp(_currentRotation, _desiredRotation, time2);

        _currentRotation = lerpTowardsDesiredRotation;
        cam.transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    #endregion

}