using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// TriggerEvent does an event to multiple objects 
/// when the player enters the trigger collider.
/// 
/// A single (repeatable if wished) event is executed.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [Header("Configuration / Dependencies")]
    //
    [SerializeField] bool actionIsRepeatable;
    [SerializeField] bool showEventListeners = true;
    [SerializeField] Color gizmoColor;
    [SerializeField] Collider trigger;
    bool _eventHasBeenExecuted;

    [Space(10)]
    [Header("Events")]
    //
    [SerializeField] UnityEvent enteredThroughTrigger;

    #region MonoBehaviour
    void OnTriggerEnter(Collider other)
    {
        bool colliderIsPlayer = other.transform.parent.TryGetComponent(out PlayerProperties player);

        if (colliderIsPlayer)
        {
            if (actionIsRepeatable)
            {
                enteredThroughTrigger?.Invoke();
            } 
            else
            {
                if (!_eventHasBeenExecuted)
                {
                    enteredThroughTrigger?.Invoke();
                    _eventHasBeenExecuted = true;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawCube(trigger.bounds.center, trigger.bounds.size);

        if (showEventListeners)
        {
            DrawLinesTowardEventListeners();
        }
    }
    #endregion

    #region Private methods

    void DrawLinesTowardEventListeners()
    {
        //pointer color removes transparency from the original gizmo color
        Color pointerColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Gizmos.color = pointerColor;

        //cycle through unity event targets and point towards their position
        int listenerCount = enteredThroughTrigger.GetPersistentEventCount();
        for (int i = 0; i < listenerCount; i++)
        {
            string targetName = enteredThroughTrigger.GetPersistentTarget(i).name;
            //an object is necessary in the inspector, if not, an exception will be thrown

            if (targetName != null)
            {
                GameObject target = GameObject.Find(targetName);
                Vector3 origin = trigger.bounds.center;
                Vector3 destination = target.transform.position;

                Gizmos.DrawLine(origin, destination);
            }
        }
    }

    #endregion
}