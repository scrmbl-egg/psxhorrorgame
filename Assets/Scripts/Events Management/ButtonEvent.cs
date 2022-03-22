using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ButtonEvent does an event to multiple objects 
/// when the player interacts with the collider.
/// 
/// A single (repeatable if wished) event is executed.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ButtonEvent : MonoBehaviour, IInteractive
{
    [Header("Configuration / Dependencies")]
    //
    [SerializeField] bool pressIsRepeatable;
    [SerializeField] bool showEventListeners = true;
    [SerializeField] Color gizmoColor;
    [SerializeField] Collider pressingArea;
    bool _eventHasBeenExecuted;

    [Space(10)]
    [Header("Events")]
    //
    [SerializeField] UnityEvent pressedButton;

    #region MonoBehaviour

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(pressingArea.bounds.center, pressingArea.bounds.size);

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
        int listenerCount = pressedButton.GetPersistentEventCount();
        for (int i = 0; i < listenerCount; i++)
        {
            string targetName = pressedButton.GetPersistentTarget(i).name;
            //an object is necessary in the inspector, if not, an exception will be thrown

            if (targetName != null)
            {
                GameObject target = GameObject.Find(targetName);
                Vector3 origin = pressingArea.bounds.center;
                Vector3 destination = target.transform.position;

                Gizmos.DrawLine(origin, destination);
            }
        }
    }

    #endregion
    #region Public methods

    #region IInteractive

    public void Interact()
    {
        if (pressIsRepeatable)
        {
            Press();
        }
        else
        {
            if (!_eventHasBeenExecuted)
            {
                Press();
                _eventHasBeenExecuted = true;
            }
            else
            {
                Debug.Log("Button can't be pressed anymore");
            }
        }
    }

    #endregion

    public void Press()
    {
        Debug.Log("Button pressed");
    }

    #endregion
}