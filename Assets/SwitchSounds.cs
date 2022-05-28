using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public class SwitchSounds : MonoBehaviour
{
    [Header( "Properties" )]
    //
    [SerializeField] private AudioClip firstPullClip;
    [SerializeField] private AudioClip secondPullClip;
    private AudioSource _audioSource;

    #region MonoBehaviour

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region Animation Events

    public void Pull1()
    {
        _audioSource.PlayOneShot( firstPullClip );
    }

    public void Pull2()
    {
        _audioSource.PlayOneShot( secondPullClip );
    }

    #endregion
}