using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] Text messageDisplay;
    [SerializeField] float charactersPerSecond;
    [SerializeField] float textFadeOutDuration;
    string _currentText;
    float _currentDisplayOpacity;

    #region MonoBehaviour

    void Update()
    {
        MessageDisplayManagement();
    }

    #endregion

    #region Public methods

    public IEnumerator TypeMessage(string message)
    {
        //reset text and opacity
        _currentText = "";
        _currentDisplayOpacity = 1f;

        char[] characters = message.ToCharArray();
        foreach (char character in characters)
        {
            float secondsPerCharacter = 1 / charactersPerSecond;

            _currentText += character;
            yield return new WaitForSeconds(secondsPerCharacter);
        }

        yield break;
    }

    #endregion
    #region Private methods

    void MessageDisplayManagement()
    {
        Color textColor = new Color(r: messageDisplay.color.r,
                                    g: messageDisplay.color.g,
                                    b: messageDisplay.color.b,
                                    a: _currentDisplayOpacity);

        messageDisplay.color = textColor;
        messageDisplay.text = _currentText;
    }

    #endregion
}