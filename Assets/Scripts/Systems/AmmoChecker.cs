using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoChecker : MonoBehaviour
{
    [Header("Properties")]
    //
    [SerializeField] private TMP_Text messageDisplay;
    [SerializeField] private float textDurationAfterPrint;
    [SerializeField, Range(float.Epsilon, 10)] private float fadeInSpeed;
    [SerializeField, Range(float.Epsilon, 10)] private float fadeOutSpeed;
    private const float FADE_SPEED_MULTIPLIER = 0.01f;
    private string _currentText;
    private float _opacity;

    #region MonoBehaviour

    private void Update()
    {
        MessageDisplayManagement();
    }

    #endregion

    #region Public methods

    public void PrintMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(PrintTextAndFade(message));
    }

    #endregion
    #region Private methods

    private void MessageDisplayManagement()
    {
        _opacity = Mathf.Clamp01(_opacity);

        Color textColor = 
            new Color(r: messageDisplay.color.r,
                      g: messageDisplay.color.g,
                      b: messageDisplay.color.b,
                      a: _opacity);

        messageDisplay.color = textColor;
        messageDisplay.text = _currentText;
    }

    private IEnumerator PrintTextAndFade(string text)
    {
        const float TARGET_OPACITY = 0.15f;

        _opacity = 0;
        _currentText = text;

        //fade in
        while (_opacity < TARGET_OPACITY)
        {
            float speed = fadeInSpeed * FADE_SPEED_MULTIPLIER * Time.deltaTime;

            _opacity += speed;
            yield return new WaitForEndOfFrame();
        }

        //hold
        yield return new WaitForSeconds(textDurationAfterPrint);

        //fade out
        while (_opacity > 0)
        {
            float speed = fadeOutSpeed * FADE_SPEED_MULTIPLIER * Time.deltaTime;

            _opacity -= speed;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    #endregion
}