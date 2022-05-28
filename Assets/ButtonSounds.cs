using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip releaseClip;
    private AudioSource _audioSource;

    #region MonoBehaviour

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region AnimationEvents

    public void Press()
    {
        _audioSource.PlayOneShot( pressClip );
    }

    public void Release()
    {
        _audioSource.PlayOneShot( releaseClip );
    }

    #endregion
}