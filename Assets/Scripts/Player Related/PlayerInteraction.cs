using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerLook))]
public class PlayerInteraction : MonoBehaviour
{
    //input
    private PlayerInputActions _playerInputActions;

    [Header("Properties / Dependencies")]
    //
    [SerializeField] private Transform playerCam;
    [SerializeField] private Image handIcon;
    [SerializeField, Min(float.Epsilon)] private float iconFadeSpeed;
    [SerializeField, Min(.5f)] private float interactionRange;
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private bool showGizmoRay;
    private const float TARGET_ICON_OPACITY = 0.75f;
    private float _iconOpacity;

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

    private void Update()
    {
        ShowHandIcon();
    }

    private void OnDrawGizmos()
    {
        if (showGizmoRay) DrawInteractionRay();
    }


    #endregion

    #region Private methods

    private void InteractDetection(InputAction.CallbackContext ctx)
    {
        if (ctx.started) PerformInteraction();
    }

    private void PerformInteraction()
    {
        Ray ray = 
            new Ray(origin: playerCam.transform.position,
                    direction: playerCam.transform.forward);
        bool objectIsNotInRange = 
            !Physics.Raycast(ray: ray,
                             hitInfo: out RaycastHit hit,
                             maxDistance: interactionRange,
                             layerMask: interactionLayers);

        if (objectIsNotInRange) return;
        //else...

        bool isInteractive = hit.transform.TryGetComponent(out IInteractive interactiveObject);
        if (isInteractive) interactiveObject.Interact(this);
    }

    private void ShowHandIcon()
    {
        Ray ray =
            new Ray(origin: playerCam.transform.position,
                    direction: playerCam.transform.forward);
        bool objectIsInRange =
            Physics.Raycast(ray: ray,
                            hitInfo: out RaycastHit hit,
                            maxDistance: interactionRange,
                            layerMask: interactionLayers);

        Color color = 
            new Color(r: handIcon.color.r,
                      g: handIcon.color.g,
                      b: handIcon.color.b,
                      a: _iconOpacity);

        float t = iconFadeSpeed * Time.deltaTime;
        float lerpTowardsTargetOpacity = Mathf.Lerp(_iconOpacity, TARGET_ICON_OPACITY, t);
        float lerpTowards0Opacity = Mathf.Lerp(_iconOpacity, 0, t);

        handIcon.color = color;

        if (objectIsInRange)
        {
            _iconOpacity = lerpTowardsTargetOpacity;
        }
        else
        {
            _iconOpacity = lerpTowards0Opacity;
        }
    }

    private void DrawInteractionRay()
    {
        Vector3 origin = playerCam.transform.position;
        Vector3 direction = playerCam.transform.forward * interactionRange;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(origin, direction);
    }

    #endregion
}
