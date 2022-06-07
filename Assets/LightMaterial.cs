using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMaterial : MonoBehaviour
{
    [Header( "Properties" )]
    //
    [SerializeField] private Material emissionMaterial;
    [SerializeField] private Material litMaterial;

    [SerializeField] private Material[] emissionMaterialsArray;
    [SerializeField] private Material[] litMaterialsArray;

    private Renderer _renderer;

    #region MonoBehaviour

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    #endregion

    #region Public methods

    public void TurnOnLightMaterial()
    {
        _renderer.materials = emissionMaterialsArray;
    }

    public void TurnOffLightMaterial()
    {
        _renderer.materials = litMaterialsArray;
    }

    #endregion
}