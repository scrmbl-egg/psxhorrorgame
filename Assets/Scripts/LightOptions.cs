using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOptions : MonoBehaviour
{
    [Header("Properties")]
    //
    [SerializeField] AnimationCurve curve;
    [SerializeField] float time;
    Light _light;

    #region MonoBehaviour

    void Awake()
    {
        _light = GetComponent<Light>();
    }

    #endregion

    #region Public methods

    public void SetLightColorR(float r)
    {
        r = Mathf.Clamp01(r);
        _light.color = new Color(r, _light.color.g, _light.color.b);
    }

    public void SetLightColorG(float g)
    {
        g = Mathf.Clamp01(g);
        _light.color = new Color(_light.color.r, g, _light.color.b);
    }

    public void SetLightColorB(float b)
    {
        b = Mathf.Clamp01(b);
        _light.color = new Color(_light.color.r, _light.color.g, b);
    }

    #endregion
}