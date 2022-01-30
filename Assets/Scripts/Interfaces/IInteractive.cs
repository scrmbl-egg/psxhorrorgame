using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfaz para clases que requieran de interacción por parte del jugador.
/// </summary>
public interface IInteractive
{
    /// <summary>
    /// Llama la interacción con el objeto dueño de la clase.
    /// </summary>
    public void Interact();
}
