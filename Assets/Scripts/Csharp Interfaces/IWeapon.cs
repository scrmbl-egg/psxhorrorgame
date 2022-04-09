using UnityEngine.InputSystem;

/// <summary>
/// Interface for weapon classes.
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// Melee attacks with the weapon.
    /// </summary>
    public void MeleeAttack();

    /// <summary>
    /// Fires weapon.
    /// </summary>
    public void Fire();
}
