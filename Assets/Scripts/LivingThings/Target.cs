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

    [HideInInspector] public TMP_Text TMPText;

    #region MonoBehaviour

    private void Awake()
    {
        _livingThing = GetComponent<LivingThing>();
        TMPText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        TMPText.text = _livingThing.Health.ToString();
    }

    private void LateUpdate()
    {
        transform.LookAt(lookingTarget, Vector3.up);
    }

    #endregion
}
