using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationOptions : MonoBehaviour
{
    [Header( "Properties" )]
    //
    [SerializeField] private int necessaryAmountOfPressesToQuit;
    [SerializeField] private float discardingAmountOfPressesDuration;

    private float _currentTime;
    private int _amountOfPresses;

    #region MonoBehaviour

    private void Update()
    {
        bool hasPressedButton = _amountOfPresses >= 1;
        if (hasPressedButton)
        {
            _currentTime += Time.deltaTime;

            bool timerIsDone = _currentTime >= discardingAmountOfPressesDuration;
            if (timerIsDone)
            {
                _currentTime = 0;
                _amountOfPresses = 0;
            }
        }
    }

    #endregion

    #region Public methods

    public void TryQuitApp()
    {
        _amountOfPresses++;

        bool mustQuit = _amountOfPresses == necessaryAmountOfPressesToQuit;
        if (mustQuit)
        {
            Application.Quit();
            Debug.Log( "Application closed." );
        }
    }

    #endregion
}