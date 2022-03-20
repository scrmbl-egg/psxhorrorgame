using UnityEngine;

public class CheatManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField, TextArea(2, 5)] string typedCharactersBuffer;
    [SerializeField] bool displayMessagesInConsole;
    const string _cheatCodesPrefix = "a3d";
    const int _maxCharactersInBuffer = 50;

    [Space(10)]
    [SerializeField] CheatCode[] cheatCodes;

    #region MonoBehaviour

    void Awake()
    {
        
    }

    void Update()
    {
        InputManagement();
        CheatCodeCheck();
    }

    #endregion

    #region Private methods

    void InputManagement()
    {
        foreach (char character in Input.inputString)
        {
            typedCharactersBuffer += character;

            bool bufferIsFull = typedCharactersBuffer.Length >= _maxCharactersInBuffer;
            if (bufferIsFull)
            {
                //remove first character
                typedCharactersBuffer = typedCharactersBuffer.Remove(0, 1);
            }
        }
    }

    void CheatCodeCheck()
    {
        foreach (CheatCode cheatCode in cheatCodes)
        {
            bool cheatCodeIsTyped = typedCharactersBuffer.ToLower().EndsWith(_cheatCodesPrefix.ToLower() + cheatCode.Code.ToLower());
            if (cheatCodeIsTyped)
            {
                cheatCode.OnCheatTyped?.Invoke();
                typedCharactersBuffer = "";

                if (displayMessagesInConsole)
                {
                    Debug.Log(cheatCode.Message);
                }
            }
        }
    }

    #endregion
}
