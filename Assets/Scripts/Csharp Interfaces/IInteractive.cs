using UnityEngine;

/// <summary>
/// Interface for classes that have player interaction.
/// </summary>
public interface IInteractive
{
    /// <summary>
    /// Calls the interaction within the owner class.
    /// </summary>
    /// <param name="sender">Component that has invoked interaction.</param>
    public void Interact(Component sender);
}