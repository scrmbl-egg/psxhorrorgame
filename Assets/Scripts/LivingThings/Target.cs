using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    [Header("Dependencies")]
    //
    [SerializeField] private Transform lookingTarget;
    private LivingThing _livingThing;
    private TMP_Text _tmpText;

    #region MonoBehaviour

    private void Awake()
    {
        _livingThing = GetComponent<LivingThing>();
        _tmpText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        _tmpText.text = _livingThing.Health.ToString();
    }

    private void LateUpdate()
    {
        transform.LookAt(lookingTarget, Vector3.up);
    }

    #endregion
}
