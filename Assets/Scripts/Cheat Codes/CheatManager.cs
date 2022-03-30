using UnityEngine;

public class CheatManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField, TextArea(2, 5)] string typedCharactersBuffer;
    private const string CHEAT_CODES_PREFIX = "id";
    private const int MAX_CHARACTERS_IN_BUFFER = 50;

    [Space(10)]
    [SerializeField] CheatCode[] cheatCodes;

    #region MonoBehaviour

    private void Awake()
    {
        
    }

    private void Update()
    {
        InputManagement();
        CheatCodeCheck();
    }

    #endregion

    #region Private methods

    private void InputManagement()
    {
        foreach (char character in Input.inputString)
        {
            typedCharactersBuffer += character;

            bool bufferIsFull = typedCharactersBuffer.Length >= MAX_CHARACTERS_IN_BUFFER;
            if (bufferIsFull)
            {
                //remove first character
                typedCharactersBuffer = typedCharactersBuffer.Remove(0, 1);
            }
        }
    }

    private void CheatCodeCheck()
    {
        foreach (CheatCode cheatCode in cheatCodes)
        {
            bool cheatCodeIsTyped = typedCharactersBuffer.ToLower().EndsWith(CHEAT_CODES_PREFIX.ToLower() + cheatCode.Code.ToLower());
            if (cheatCodeIsTyped)
            {
                cheatCode.OnCheatTyped?.Invoke();
                typedCharactersBuffer = "";
            }
        }
    }

    #endregion
}
