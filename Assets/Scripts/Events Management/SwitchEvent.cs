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
    [SerializeField] bool showEventListeners = true;
    [SerializeField] Color gizmoColor;
    [SerializeField] Collider pressingArea;
    bool _switchIsPulled;

    [Space(10)]
    [Header("Events")]
    //
    [SerializeField] UnityEvent onSwitchStateOne;
    [SerializeField] UnityEvent onSwitchStateTwo;

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

    void PullInteraction()
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

    void FirstPull()
    {
        onSwitchStateOne?.Invoke();
        _switchIsPulled = true;
    }

    void SecondPull()
    {
        onSwitchStateTwo?.Invoke();
        _switchIsPulled = false;
    }

    void DrawLinesTowardEventListeners()
    {
        #region State One Listeners

        //first pointer color removes transparency from the original gizmo color
        Color stateOneListenerColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Gizmos.color = stateOneListenerColor;

        //cycle through unity event targets and point towards their position
        int stateOneListenersCount = onSwitchStateOne.GetPersistentEventCount();
        for (int i = 0; i < stateOneListenersCount; i++)
        {
            string targetName = onSwitchStateOne.GetPersistentTarget(i).name;
            //an object is necessary in the inspector, if not, an exception will be thrown

            if (targetName != null)
            {
                GameObject target = GameObject.Find(targetName);
                Vector3 origin = pressingArea.bounds.center;
                Vector3 destination = target.transform.position;

                Gizmos.DrawLine(origin, destination);
            }
        }

        #endregion
        #region State Two Listeners

        //second pointer color removes transparency from the original gizmo color and inverts it
        Color stateTwoListenerColor = new Color(1 - gizmoColor.r, 1 - gizmoColor.g, 1 - gizmoColor.b, 1);
        Gizmos.color = stateTwoListenerColor;

        //cycle through unity event targets and point towards their position
        int stateTwoListenersCount = onSwitchStateTwo.GetPersistentEventCount();
        for (int i = 0; i < stateTwoListenersCount; i++)
        {
            string targetName = onSwitchStateTwo.GetPersistentTarget(i).name;
            //an object is necessary in the inspector, if not, an exception will be thrown

            if (targetName != null)
            {
                GameObject target = GameObject.Find(targetName);
                Vector3 origin = pressingArea.bounds.center;
                Vector3 destination = target.transform.position;

                Gizmos.DrawLine(origin, destination);
            }
        }

        #endregion
    }

    #endregion
    #region Public methods

    #region IInteractive

    public void Interact()
    {
        PullInteraction();
    }

    #endregion



    #endregion
}