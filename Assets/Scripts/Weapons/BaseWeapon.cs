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
    [SerializeField] private Transform raycastOrigin;
    [Space(2)]
    [SerializeField, Min(1)] private int pelletsPerShot = 1;
    [Space(2)]
    [SerializeField, Min(1)] private int meleeDamage;
    [SerializeField, Min(1)] private int minWeaponDamage;
    [SerializeField, Min(1)] private int maxWeaponDamage;
    [Space(2)]
    [SerializeField, Min(float.Epsilon)] private float meeleeRange;
    [SerializeField, Min(float.Epsilon)] private float meleeForce;
    [SerializeField, Min(float.Epsilon)] private float weaponRange;
    [SerializeField, Min(float.Epsilon)] private float weaponForce;
    [Space(2)]
    [SerializeField, Range(0f, 45f)] private float horizontalSpread;
    [SerializeField, Range(0f, 45f)] private float verticalSpread;
    [Space(2)]
    [SerializeField] private LayerMask raycastLayers;
    [SerializeField] private GameObject[] bulletHoleDecals;

    public string WeaponName => weaponName;
    public Transform RaycastOrigin => raycastOrigin;
    public int PelletsPerShot => pelletsPerShot;
    public int MeleeDamage => meleeDamage;
    public int WeaponDamage => Random.Range(minWeaponDamage, maxWeaponDamage + 1);
    public float MeleeRange => meeleeRange;
    public float MeleeForce => meleeForce;
    public float WeaponRange => weaponRange;
    public float WeaponForce => weaponForce;
    public LayerMask RaycastLayers => raycastLayers;
    public GameObject[] BulletHoleDecals => bulletHoleDecals;

    #region Public methods

    public Vector3 ApplySpreadToDirection(Vector3 vector)
    {
        float hDegrees = horizontalSpread / 2;
        float vDegrees = verticalSpread / 2;

        float randomHorizontalSpread = Random.Range(-hDegrees, hDegrees);
        float randomVerticalSpread = Random.Range(-vDegrees, vDegrees);

        vector = Quaternion.AngleAxis(randomHorizontalSpread, RaycastOrigin.up) * vector; //horizontal spread
        vector = Quaternion.AngleAxis(randomVerticalSpread, RaycastOrigin.right) * vector; //vertical spread

        return vector;
    }

    public void SpawnRandomBulletHole(RaycastHit hitInfo)
    {
        int randomBulletHole = Random.Range(0, BulletHoleDecals.Length);
        GameObject bulletHole = Instantiate(original: BulletHoleDecals[randomBulletHole],
                                            position: hitInfo.point,
                                            rotation: Quaternion.LookRotation(hitInfo.normal));
        bulletHole.transform.parent = hitInfo.transform;
    }

    public void PushRigidbodyFromRaycastHit(RaycastHit hitInfo, float force)
    {
        bool rigidbodyIsNotDetected = hitInfo.rigidbody == null;
        if (rigidbodyIsNotDetected) return;
        //else...

        Rigidbody targetRigidbody = hitInfo.rigidbody;
        Vector3 forceVector = -hitInfo.normal * force;

        bool rigidbodyIsNotKinematic = !targetRigidbody.isKinematic;
        if (rigidbodyIsNotKinematic) 
            targetRigidbody.AddForce(forceVector, ForceMode.Impulse);
    }

    public void GetLivingThingFromTransform(Transform transform, out LivingThing livingThing)
    {
        livingThing = null;

        bool parentIsNotLivingThing = true;
        while (parentIsNotLivingThing)
        {
            bool currentTransformIsNotLivingThing = !transform.TryGetComponent(out livingThing);

            if (currentTransformIsNotLivingThing)
                //go to parent transform and reiterate loop from there
                transform = transform.parent;
            else
                //end loop
                parentIsNotLivingThing = false;
        }
    }
    #endregion
}