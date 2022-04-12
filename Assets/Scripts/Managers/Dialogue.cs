using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("Properties")]
    //
    [SerializeField] private TMP_Text messageDisplay;
    [SerializeField] private float textDurationAfterTyping;
    [SerializeField, Range(0, 1)] private float fadeInSpeed;
    [SerializeField, Range(0, 1)] private float fadeOutSpeed;
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

        //return color to white if changed
        Color resetColor = new Color(r: 1,
                                     g: 1,
                                     b: 1,
                                     a: _opacity);

        messageDisplay.color = resetColor;
        StartCoroutine(PrintTextAndFade(message));
    }

    #endregion
    #region Private methods

    private void MessageDisplayManagement()
    {
        _opacity = Mathf.Clamp01(_opacity);

        Color textColor = new Color(r: messageDisplay.color.r,
                                    g: messageDisplay.color.g,
                                    b: messageDisplay.color.b,
                                    a: _opacity);

        messageDisplay.color = textColor;
        messageDisplay.text = _currentText;
    }

    private IEnumerator PrintTextAndFade(string text)
    {
        //HACK: fade in and fade out
        _currentText = string.Empty;
        _currentText = text;
        _opacity = 0;

        while (_opacity < 1f)
        {
            float time = Time.deltaTime * fadeInSpeed;
            _opacity += time;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(textDurationAfterTyping);

        while (_opacity > 0)
        {
            float time = Time.deltaTime * fadeOutSpeed;
            _opacity -= time;
            yield return new WaitForSeconds(time);
        }

        yield break;
    }

    #endregion
}