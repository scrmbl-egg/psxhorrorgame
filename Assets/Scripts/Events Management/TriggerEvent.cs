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
    [SerializeField] private bool actionIsRepeatable;
    [SerializeField] private bool showEventListeners = true;
    [SerializeField] private Color gizmoColor;
    [SerializeField] private Collider trigger;
    private bool _eventHasBeenExecuted;

    [Space(10)]
    [Header("Events")]
    //
    [SerializeField] private UnityEvent enteredThroughTrigger;

    #region MonoBehaviour
    private void OnTriggerEnter(Collider other)
    {
        bool colliderIsPlayer = other.CompareTag("Player");

        if (colliderIsPlayer) InvokeEvents();
    }

    private void InvokeEvents()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(trigger.bounds.center, trigger.bounds.size);

        if (showEventListeners) DrawLinesTowardEventListeners();
    }
    #endregion

    #region Private methods

    private void DrawLinesTowardEventListeners()
    {
        //pointer color removes transparency from the original gizmo color
        Color pointerColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Gizmos.color = pointerColor;

        //cycle through unity event targets and point towards their position
        Vector3 origin = trigger.bounds.center;

        int listenerCount = enteredThroughTrigger.GetPersistentEventCount();
        for (int i = 0; i < listenerCount; i++)
        {
            Object targetObject = enteredThroughTrigger.GetPersistentTarget( i );

            if (targetObject == null) continue;
            //else...

            GameObject target = GameObject.Find( targetObject.name );
            Vector3 destination = target.transform.position;

            Gizmos.DrawLine( origin, destination );
        }
    }

    #endregion
}