using UnityEngine.InputSystem;

/// <summary>
/// Interface for weapon classes.
/// </summary>
public interface IWeapon
{
    public bool IsAiming { get; set; }
    public bool IsCovering { get; set; }

    /// <summary>
    /// Melee attacks with the weapon.
    /// </summary>
    public void MeleeAttack();

    /// <summary>
    /// Fires weapon.
    /// </summary>
    public void Fire();
}
