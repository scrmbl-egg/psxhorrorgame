using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour, IInteractive
{
    [Header("Dependencies")]
    //
    [SerializeField] private Collider pressingArea;

    [Space(10)]
    [Header("Properties")]
    //
    [SerializeField, Range(1, 100)] private int id;

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool showDoorsWithSameKeyIds;
    [SerializeField] private bool showPressingArea;
    [SerializeField] private Color gizmoColor;

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        if (showPressingArea) DrawInteractionArea();
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
        Destroy(gameObject);
    }

    private void DrawInteractionArea()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawCube(pressingArea.bounds.center, pressingArea.bounds.size);
    }

    private void DrawLinesTowardDoorsWithSameIDs()
    {
        //pointer color removes transparency from the original gizmo color
        Color pointerColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Gizmos.color = pointerColor;

        //cycle through doors with the same key ID and point towards them
        Vector3 lineOrigin = pressingArea.bounds.center;

        Door[] doors = FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
        {
            bool doorIDsAreNotTheSame = doors[i].KeyID != id;
            if (doorIDsAreNotTheSame) continue;
            //else...

            Vector3 destination = doors[i].transform.position;
            Gizmos.DrawLine(lineOrigin, destination);
        }
    }

    #endregion
}