using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("Properties")]
    //
    [SerializeField] private TMP_Text messageDisplay;
    [SerializeField, Min(1)] private float charactersPerSecond;
    [SerializeField] private float textDurationAfterTyping;
    [SerializeField, Range(float.Epsilon, 25)] private float fadeOutSpeed;
    private const float FADE_SPEED_MULTIPLIER = 0.01f;
    private Queue<string> _messageQueue = new Queue<string>();
    private string _currentText;
    private float _opacity;

    #region MonoBehaviour

    private void Update()
    {
        MessageDisplayManagement();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Prints a message and clears the current queue.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public void PrintMessage(string message)
    {
        StopAllCoroutines();

        if (_messageQueue.Count > 0) _messageQueue.Clear();

        StartCoroutine(TypeMessage(message));
    }

    /// <summary>
    /// Queues a message for later printing.
    /// </summary>
    /// <param name="message">Queued message.</param>
    public void QueueMessage(string message)
    {
        _messageQueue.Enqueue(message);
    }

    #endregion
    #region Private methods

    private void MessageDisplayManagement()
    {
        _opacity = Mathf.Clamp01(_opacity);

        Color color = 
            new Color(r: messageDisplay.color.r,
                      g: messageDisplay.color.g,
                      b: messageDisplay.color.b,
                      a: _opacity);

        messageDisplay.color = color;
        messageDisplay.text = _currentText;
    }

    private IEnumerator TypeMessage(string message)
    {
        const float TARGET_OPACITY = 0.15f;

        float secondsPerCharacter = 1 / charactersPerSecond;
        char[] characters = message.ToCharArray();

        _opacity = TARGET_OPACITY;
        _currentText = string.Empty;

        //type
        foreach (char character in characters)
        {
            _currentText += character;
            yield return new WaitForSeconds(secondsPerCharacter);
        }

        //hold
        yield return new WaitForSeconds(textDurationAfterTyping);

        //fade out
        while (_opacity > 0)
        {
            float speed = fadeOutSpeed * FADE_SPEED_MULTIPLIER * Time.deltaTime;

            _opacity -= speed;
            yield return new WaitForEndOfFrame();
        }

        if (_messageQueue.Count > 0)
        {
            PrintNextMessageInQueue();
        }

        yield break;
    }

    private void PrintNextMessageInQueue()
    {
        StopAllCoroutines();

        StartCoroutine(TypeMessage(_messageQueue.Peek()));
        _messageQueue.Dequeue();
    }

    #endregion
}