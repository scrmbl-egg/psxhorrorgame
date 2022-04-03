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
    [SerializeField] private int meleeDamage;
    [SerializeField] private int minWeaponDamage;
    [SerializeField] private int maxWeaponDamage;
    [Space(2)]
    [SerializeField] private float meeleeRange;
    [SerializeField] private float weaponRange;
    [Space(2)]
    [SerializeField, Range(0, 90)] private float horizontalSpread;
    [SerializeField, Range(0, 90)] private float verticalSpread;
    [Space(2)]
    [SerializeField] private LayerMask raycastLayers;

    public string WeaponName => weaponName;
    public int MeleeDamage => meleeDamage;
    public int WeaponDamage => Random.Range(minWeaponDamage, maxWeaponDamage + 1);
    public float MeleeRange => meeleeRange;
    public float WeaponRange => weaponRange;

    public LayerMask RaycastLayers => raycastLayers;

    #region Public methods

    public Vector3 RandomSpread(Vector3 vector)
    {
        Vector3 spreadVector = vector;
        float randomHorizontalSpread = Random.Range(-horizontalSpread, horizontalSpread);
        float randomVerticalSpread = Random.Range(-verticalSpread, verticalSpread);

        spreadVector = Quaternion.AngleAxis(randomHorizontalSpread, Vector3.up) * spreadVector; //horizontal spread
        spreadVector = Quaternion.AngleAxis(randomVerticalSpread, Vector3.right) * spreadVector; //vertical spread

        return spreadVector;
    }

    #endregion
}