using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] TMP_Text messageDisplay;
    [SerializeField] float charactersPerSecond;
    [SerializeField] float textDurationAfterTyping;
    [SerializeField, Range(0f, 20f)] float fadeOutSpeed;
    string _currentText;
    float _currentDisplayOpacity;

    [Header("Fun Stuff")]
    [SerializeField] Color cheatMessageColor;

    #region MonoBehaviour

    void Update()
    {
        MessageDisplayManagement();
    }

    #endregion

    #region Public methods

    public void TypeMessage(string message)
    {
        StopAllCoroutines();

        //return color to white if changed
        Color resetColor = new Color(r: 1,
                                     g: 1,
                                     b: 1,
                                     a: _currentDisplayOpacity);

        messageDisplay.color = resetColor;
        StartCoroutine(TypeEffect(message));
    }

    public void TypeCheatMessage(string message)
    {
        StopAllCoroutines();

        messageDisplay.color = new Color(r: cheatMessageColor.r,
                                         g: cheatMessageColor.g,
                                         b: cheatMessageColor.b,
                                         a: _currentDisplayOpacity);

        StartCoroutine(TypeEffect(message));
    }

    #endregion
    #region Private methods

    void MessageDisplayManagement()
    {
        _currentDisplayOpacity = Mathf.Clamp01(_currentDisplayOpacity);

        Color textColor = new Color(r: messageDisplay.color.r,
                                    g: messageDisplay.color.g,
                                    b: messageDisplay.color.b,
                                    a: _currentDisplayOpacity);

        messageDisplay.color = textColor;
        messageDisplay.text = _currentText;
    }

    IEnumerator TypeEffect(string text)
    {
        //empty text and put full opacity
        _currentText = string.Empty;
        _currentDisplayOpacity = 1f;

        //typing speed
        float secondsPerCharacter = 1 / charactersPerSecond;

        //divide text and type it slowly
        char[] characters = text.ToCharArray();
        foreach (char character in characters)
        {
            _currentText += character;
            yield return new WaitForSeconds(secondsPerCharacter);
        }

        //wait some time
        yield return new WaitForSeconds(textDurationAfterTyping);

        //fade out
        while (_currentDisplayOpacity > 0)
        {
            _currentDisplayOpacity -= Time.deltaTime * fadeOutSpeed;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    #endregion
}