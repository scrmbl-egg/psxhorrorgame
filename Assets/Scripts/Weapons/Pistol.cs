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
    [SerializeField] private GameObject bulletHoleDecal;

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
    }

    #endregion
    #region Private methods

    #endregion
}