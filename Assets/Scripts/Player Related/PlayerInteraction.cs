using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLook))]
public class PlayerInteraction : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header("Properties / Dependencies")]
    //
    [SerializeField] private Transform playerCam;
    [SerializeField, Min(.5f)] private float interactionRange;
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private bool showGizmoRay;

    #region MonoBehaviour

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        //input
        _playerInputActions.PlayerThing.Interact.Enable();
        _playerInputActions.PlayerThing.Interact.started += InteractDetection;
    }

    private void OnDisable()
    {
        //input
        _playerInputActions.PlayerThing.Interact.Disable();
        _playerInputActions.PlayerThing.Interact.started -= InteractDetection;
    }

    private void OnDrawGizmos()
    {
        if (showGizmoRay)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(from: playerCam.transform.position,
                           direction: playerCam.transform.forward * interactionRange);
        }
    }

    #endregion

    #region Private methods

    private void InteractDetection(InputAction.CallbackContext ctx)
    {
        if (ctx.started) PerformInteraction();
    }

    private void PerformInteraction()
    {
        Ray ray = new Ray(origin: playerCam.transform.position,
                          direction: playerCam.transform.forward);
        bool objectIsNotInRange = !Physics.Raycast(ray: ray,
                                                   hitInfo: out RaycastHit hit,
                                                   maxDistance: interactionRange,
                                                   layerMask: interactionLayers);

        if (objectIsNotInRange) return;
        //else...

        bool isInteractive = hit.transform.TryGetComponent(out IInteractive interactiveObject);
        if (isInteractive) interactiveObject.Interact(this);
    }

    #endregion
}
