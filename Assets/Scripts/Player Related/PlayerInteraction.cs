using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerLook))]
public class PlayerInteraction : MonoBehaviour
{
    //TODO: Make a global input class or make use of the new input system and delete this header
    [Header("Input")]
    //
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("Properties / Dependencies")]
    //
    [SerializeField] private Camera playerCamera;
    [SerializeField, Min(.5f)] private float interactionRange;
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private bool showGizmoRay;

    #region MonoBehaviour

    private void Update()
    {
        InputManagement();
    }

    private void OnDrawGizmos()
    {
        if (showGizmoRay)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionRange);
        }
    }

    #endregion

    #region Private methods

    private void InputManagement()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            bool objectIsInRange = Physics.Raycast(origin: playerCamera.transform.position,
                                                   direction: playerCamera.transform.forward,
                                                   hitInfo: out RaycastHit hit,
                                                   maxDistance: interactionRange,
                                                   layerMask: interactionLayers);

            if (objectIsInRange)
            {
                bool isInteractive = hit.collider.TryGetComponent(out IInteractive interactiveObject);
                if (isInteractive)
                {
                    interactiveObject.Interact();
                }
            }
        }
    }

    #endregion
}
