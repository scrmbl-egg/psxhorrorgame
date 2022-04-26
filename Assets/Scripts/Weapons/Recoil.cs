using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Testing Properties")]
    //
    [SerializeField] private float recoilXMultiplier;
    [SerializeField] private float recoilYMultiplier;
    [SerializeField] private float recoilZMultiplier;

    [ContextMenu("Recoil")]
    public void ApplyRecoil()
    {

    }
}
