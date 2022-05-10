using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private Collider interactionArea;
    private static DialogueSystem _dialogue;

    [Space(10)]
    [Header("Properties")]
    //
    [SerializeField, Range(1, 100)] private int id;
    [SerializeField, TextArea(1, 3)] private string pickupMessage;

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool showDoorsWithSameKeyIds;
    [SerializeField] private bool showInteractionArea;
    [SerializeField] private Color gizmoColor;

    #region MonoBehaviour

    private void Awake()
    {
        if (_dialogue == null) _dialogue = FindObjectOfType<DialogueSystem>();
    }

    private void OnDrawGizmos()
    {
        if (showInteractionArea) DrawInteractionArea();
        if (showDoorsWithSameKeyIds) DrawLinesTowardDoorsWithSameIDs();
    }

    #endregion

    #region Public methods

    #region IInteractive

    public void Interact(Component sender)
    {
        AddToInventory(sender);
    }

    #endregion

    #endregion
    #region Private methods

    private void AddToInventory(Component sender)
    {
        bool senderDoesntHaveInventory = !sender.TryGetComponent(out PlayerInventory inventory);
        if (senderDoesntHaveInventory) return;
        //else...

        inventory.AddKey(id);
        _dialogue.PrintMessage(pickupMessage);

        Destroy(gameObject);
    }

    private void DrawInteractionArea()
    {
        Vector3 origin = interactionArea.bounds.center;
        Vector3 size = interactionArea.bounds.size;

        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(origin, size);
    }

    private void DrawLinesTowardDoorsWithSameIDs()
    {
        //pointer color removes transparency from the original gizmo color
        Color pointerColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Gizmos.color = pointerColor;

        //cycle through doors with the same key ID and point towards them
        Vector3 origin = interactionArea.bounds.center;

        Door[] doors = FindObjectsOfType<Door>();
        foreach (Door door in doors)
        {
            bool doorIDsAreNotTheSame = door.KeyID != id;
            if (doorIDsAreNotTheSame) continue;
            //else...

            Vector3 destination = door.transform.position;
            Gizmos.DrawLine(origin, destination);
        }
    }

    #endregion
}