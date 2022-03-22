using UnityEngine;
using DG.Tweening;

public class DOTweenEvents
{
    /// <summary>
    /// Simple rotation with animation curve.
    /// </summary>
    /// <param name="_Rb"></param>
    /// <param name="_degrees"></param>
    /// <param name="_speed"></param>
    /// <param name="_animCurve"></param>
    public static void SimpleRotate(Rigidbody _Rb, Vector3 _degrees, float _speed, AnimationCurve _animCurve)
    {
        DOTweenModulePhysics.DORotate(_Rb, _degrees, 10 - _speed, RotateMode.Fast).SetEase(_animCurve);
    }

    /// <summary>
    /// Simple infinite rotation with animation curve.
    /// </summary>
    /// <param name="_Rb"></param>
    /// <param name="_speed"></param>
    /// <param name="_animCurve"></param>
    public static void SimpleRotateLoop(Rigidbody _Rb, Vector3 _degrees, float _speed, AnimationCurve _animCurve)
    {
        DOTweenModulePhysics.DORotate(_Rb, _degrees, 10 - _speed, RotateMode.Fast).SetLoops(-1).SetEase(_animCurve);
    }

    //public static string CallString(DOTweenEventType DType)
    //{
    //    string TXT = DType.ToString();
    //    return TXT;
    //}
}

public enum DOTweenEventType
{
    SimpleRotate,
    SimpleRotateLoop
}
