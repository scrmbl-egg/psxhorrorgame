using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
public class PlayerInteraction : MonoBehaviour
{
    //TODO: Make a global input class or make use of the new input system and delete this header
    [Header("Input")]
    //
    [SerializeField] KeyCode interactionKey = KeyCode.E;

    [Header("Properties / Dependencies")]
    //
    [SerializeField] Camera playerCamera;
    [SerializeField, Min(.5f)] float interactionRange;
    [SerializeField] LayerMask interactionLayers;
    [SerializeField] bool showGizmoRay;

    #region MonoBehaviour

    void Update()
    {
        InputManagement();
    }

    void OnDrawGizmos()
    {
        if (showGizmoRay)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionRange);
        }
    }

    #endregion

    #region Private methods

    void InputManagement()
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
