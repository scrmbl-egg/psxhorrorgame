using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField, Range(0, 10)] float speed;
    [SerializeField] Vector3 rotationDegrees;
    [SerializeField] DOTweenEventType dEventType;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();

        if (dEventType == DOTweenEventType.SimpleRotate)
        {
            DOTweenEvents.SimpleRotate(_rigidBody, rotationDegrees, speed, animationCurve);
        }

        if (dEventType == DOTweenEventType.SimpleRotateLoop)
        {
            DOTweenEvents.SimpleRotateLoop(_rigidBody, rotationDegrees, speed, animationCurve);
        }
    }
}
