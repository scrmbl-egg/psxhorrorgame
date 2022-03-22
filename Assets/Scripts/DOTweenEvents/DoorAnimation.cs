using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] AnimationCurve _animCurve;

    [Range(0, 10)]
    [SerializeField] float _speed;

    [SerializeField] Vector3 _degrees;

    [SerializeField] DOTweenEventType _DType;

    private Rigidbody _Rb;

    private void Awake()
    {
        _Rb = gameObject.GetComponent<Rigidbody>();

        string TEvent = _DType.ToString();

    if (_DType == DOTweenEventType.SimpleRotate)
        {
            DOTweenEvents.SimpleRotate(_Rb, _degrees, _speed, _animCurve);
        }

    if (_DType == DOTweenEventType.SimpleRotateLoop)
        {
            DOTweenEvents.SimpleRotateLoop(_Rb, _degrees, _speed, _animCurve);
        }
    }
}
