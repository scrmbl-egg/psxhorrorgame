using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// SwitchEvent does two different events to multiple objects 
/// when the switch is pulled.
/// 
/// When the switch is pulled and one event is executed,
/// the switch is prepared to execute the other next time it's pulled, 
/// cycling again and again.
/// </summary>
[RequireComponent(typeof(Collider))]
public class SwitchEvent : MonoBehaviour, IInteractive
{
    [Header("Configuration / Dependencies")]
    //
    [SerializeField] private bool showEventListeners = true;
    [SerializeField] private Color gizmoColor;
    [SerializeField] private Collider interactionArea;
    private bool _switchIsPulled;

    [Space(10)]
    [Header("Events")]
    //
    [SerializeField] private UnityEvent onSwitchStateOne;
    [SerializeField] private UnityEvent onSwitchStateTwo;
    [SerializeField] private Vector3 gizmoStateTwoOffset;

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(interactionArea.bounds.center, interactionArea.bounds.size);

        if (showEventListeners) DrawLinesTowardEventListeners();
    }

    #endregion

    #region Private methods

    private void PullInteraction()
    {
        if (_switchIsPulled)
        {
            SecondPull();
        }
        else
        {
            FirstPull();
        }
    }

    private void FirstPull()
    {
        onSwitchStateOne?.Invoke();
        _switchIsPulled = true;
    }

    private void SecondPull()
    {
        onSwitchStateTwo?.Invoke();
        _switchIsPulled = false;
    }

    private void DrawLinesTowardEventListeners()
    {
        #region State One Listeners

        //first pointer color removes transparency from the original gizmo color
        Color stateOneListenerColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Gizmos.color = stateOneListenerColor;

        //cycle through unity event targets and point towards their position
        Vector3 lineOrigin1 = interactionArea.bounds.center;

        int stateOneListenersCount = onSwitchStateOne.GetPersistentEventCount();
        for (int i = 0; i < stateOneListenersCount; i++)
        {
            string targetName = onSwitchStateOne.GetPersistentTarget(i).name;
            //an object is necessary in the inspector, if not, an exception will be thrown

            if (targetName != null)
            {
                GameObject target = GameObject.Find(targetName);
                Vector3 destination = target.transform.position;

                Gizmos.DrawLine(lineOrigin1, destination);
            }
        }

        #endregion
        #region State Two Listeners

        //second pointer color removes transparency from the original gizmo color and inverts it
        Color stateTwoListenerColor = new Color(1 - gizmoColor.r, 1 - gizmoColor.g, 1 - gizmoColor.b, 1);
        Gizmos.color = stateTwoListenerColor;

        //cycle through unity event targets and point towards their position with an offset
        Vector3 lineOrigin2 = interactionArea.bounds.center + gizmoStateTwoOffset;

        int stateTwoListenersCount = onSwitchStateTwo.GetPersistentEventCount();
        for (int i = 0; i < stateTwoListenersCount; i++)
        {
            //WARNING: EXPECT EXCEPTIONS WHEN SETTING UP THE INSPECTOR. IT'S COMPLETELY FINE.

            string targetName = onSwitchStateTwo.GetPersistentTarget(i).name;
            if (targetName != null)
            {
                GameObject target = GameObject.Find(targetName);
                Vector3 destination = target.transform.position + gizmoStateTwoOffset;

                Gizmos.DrawLine(lineOrigin2, destination);
            }
        }

        #endregion
    }

    #endregion
    #region Public methods

    #region IInteractive

    public void Interact(Component sender)
    {
        PullInteraction();
    }

    #endregion

    #endregion
}