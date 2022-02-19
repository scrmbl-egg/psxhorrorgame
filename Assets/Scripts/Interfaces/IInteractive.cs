using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for classes that require player interaction.
/// </summary>
public interface IInteractive
{
    /// <summary>
    /// Calls the interaction within the owner class.
    /// </summary>
    public void Interact();
}
