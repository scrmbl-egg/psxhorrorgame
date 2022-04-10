using UnityEngine;
using DG.Tweening;

public class DOTweenEvents
{
    /// <summary>
    /// Simple rotation with animation curve.
    /// </summary>
    /// <param name="rigidbody">Rigidbody to move</param>
    /// <param name="rotationDegrees">Target rotation relative to the current rotation</param>
    /// <param name="time">Time of the animation</param>
    /// <param name="animationCurve">Animation curve</param>
    public static void SimpleRotate(Rigidbody rigidbody, Vector3 rotationDegrees, float time, AnimationCurve animationCurve)
    {
        DOTweenModulePhysics.DORotate(rigidbody, rotationDegrees, time, RotateMode.Fast).SetEase(animationCurve);
    }

    /// <summary>
    /// Simple infinite rotation with animation curve.
    /// </summary>
    /// <param name="rigidBody">Rigidbody to move</param>
    /// <param name="time">Time of the animation</param>
    /// <param name="animationCurve">Animation curve</param>
    public static void SimpleRotateLoop(Rigidbody rigidBody, Vector3 rotationDegrees, float time, AnimationCurve animationCurve)
    {
        DOTweenModulePhysics.DORotate(rigidBody, rotationDegrees, time, RotateMode.Fast).SetLoops(-1).SetEase(animationCurve);
    }

    /// <summary>
    /// Bobbing of the character.
    /// </summary>
    /// <param name="transform">Trasform to move</param>
    /// <param name="lastPosition">Last position of animation</param>
    /// <param name="time">Time of the animation</param>
    /// <param name="animationCurve">Animation curve</param>
    public static void Bobbing(Transform transform, Vector3 lastPosition, float time, AnimationCurve animationCurve)
    {
        transform.DOMove(lastPosition, time).SetLoops(-1).SetEase(animationCurve);
    }
}