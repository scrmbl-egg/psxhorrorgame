using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLivingThing : LivingThing
{
    [Header( "Other" )]
    //
    [SerializeField] private float deathDuration;

    private Target _targetTextScript;
    private Collider _collider;
    private Renderer _renderer;

    private float _currentTime;

    #region MonoBehaviour

    private void Awake()
    {
        _targetTextScript = GetComponent<Target>();
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        ManageDeathTimer();
    }

    #endregion

    #region Public methods

    public override void DeathEffect()
    {
        _renderer.enabled = false;
        _collider.enabled = false;
        _targetTextScript.TMPText.enabled = false;
    }

    #endregion
    #region Private methods

    private void ManageDeathTimer()
    {
        if (Health > 0) return;
        //else...

        _currentTime += Time.deltaTime;

        if (_currentTime >= deathDuration)
        {
            _currentTime = 0;

            Health = MaxHealth;
            _renderer.enabled = true;
            _collider.enabled = true;
            _targetTextScript.TMPText.enabled = true;
        }
    }

    #endregion
}