
/// <summary>
/// Interface specific for guns. It
/// includes ammo checking and reloading.
/// </summary>
public interface IGun
{
    /// <summary>
    /// Reloads gun.
    /// </summary>
    public void Reload();

    /// <summary>
    /// Checks ammunition.
    /// </summary>
    public void CheckAmmo();
}
