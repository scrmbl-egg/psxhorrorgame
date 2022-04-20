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
    [SerializeField] private bool pressIsRepeatable;
    [SerializeField] private bool showEventListeners = true;
    [SerializeField] private Color gizmoColor;
    [SerializeField] private Collider interactionArea;
    private bool _eventHasBeenExecuted;

    [Space(10)]
    [Header("Events")]
    //
    [SerializeField] private UnityEvent pressedButton;

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(interactionArea.bounds.center, interactionArea.bounds.size);

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
        Vector3 lineOrigin = interactionArea.bounds.center;

        int listenerCount = pressedButton.GetPersistentEventCount();
        for (int i = 0; i < listenerCount; i++)
        {
            string targetName = pressedButton.GetPersistentTarget(i).name;
            if (targetName == null) continue;
            //else...

            //WARNING: EXPECT EXCEPTIONS WHEN SETTING UP IN THE INSPECTOR. IT'S COMPLETELY FINE.

            GameObject target = GameObject.Find(targetName);
            Vector3 destination = target.transform.position;

            Gizmos.DrawLine(lineOrigin, destination);
        }
    }

    #endregion
    #region Public methods

    #region IInteractive

    public void Interact(Component sender)
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
        pressedButton?.Invoke();
    }

    #endregion
}