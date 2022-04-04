using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic weapon class.
/// </summary>
public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    //
    [SerializeField] private string weaponName;
    [Space(2)]
    [SerializeField] private Transform shotOrigin;
    [Space(2)]
    [SerializeField] private int pelletsPerShot;
    [Space(2)]
    [SerializeField] private int meleeDamage;
    [SerializeField] private int minWeaponDamage;
    [SerializeField] private int maxWeaponDamage;
    [Space(2)]
    [SerializeField] private float meeleeRange;
    [SerializeField] private float weaponRange;
    [Space(2)]
    [SerializeField, Range(0f, 45f)] private float horizontalSpread;
    [SerializeField, Range(0f, 45f)] private float verticalSpread;
    [Space(2)]
    [SerializeField] private LayerMask raycastLayers;

    public string WeaponName => weaponName;
    public Transform ShotOrigin => shotOrigin;
    public int MeleeDamage => meleeDamage;
    public int WeaponDamage => Random.Range(minWeaponDamage, maxWeaponDamage + 1);
    public float MeleeRange => meeleeRange;
    public float WeaponRange => weaponRange;
    public LayerMask RaycastLayers => raycastLayers;

    #region Public methods

    public Vector3 RandomSpread(Vector3 vector)
    {
        Vector3 spreadVector = vector;
        float hDegrees = horizontalSpread / 2;
        float vDegrees = verticalSpread / 2;

        float randomHorizontalSpread = Random.Range(-hDegrees, hDegrees);
        float randomVerticalSpread = Random.Range(-vDegrees, vDegrees);

        spreadVector = Quaternion.AngleAxis(randomHorizontalSpread, Vector3.up) * spreadVector; //horizontal spread
        spreadVector = Quaternion.AngleAxis(randomVerticalSpread, Vector3.right) * spreadVector; //vertical spread

        return spreadVector;
    }

    #endregion
}