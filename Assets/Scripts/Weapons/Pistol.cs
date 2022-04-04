using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MagazinesGun
{
    [Space(10)]
    [Header("Dependencies")]
    //
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject BulletHole;

    #region MonoBehaviour

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) AddMagazine(Random.Range(1, MaxMagazineCapacity + 1));
        if (Input.GetButtonDown("Fire1")) Fire();
    }

    #endregion

    #region Public methods

    [ContextMenu("Fire Pistol")]
    public override void Fire()
    {
        base.Fire();

        Ray shotRay = new Ray(origin: playerCam.position,
                              direction: RandomSpread(playerCam.forward));
        bool objectIsReached = Physics.Raycast(ray: shotRay,
                                               hitInfo: out RaycastHit hit,
                                               maxDistance: WeaponRange,
                                               layerMask: RaycastLayers);

        if (objectIsReached)
        {
            bool enemyIsHit = hit.transform.TryGetComponent(out EnemyThing enemy);
            if (enemyIsHit)
            {
                enemy.Health -= WeaponDamage;
            }

            Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            BulletHole.transform.parent = hit.transform;

            Debug.Log(hit.point);
        }
    }

    #endregion
    #region Private methods

    #endregion
}