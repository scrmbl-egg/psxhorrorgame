using UnityEngine.Events;

[System.Serializable]
public class CheatCode
{
    public string Code;
    public string Message;
    public UnityEvent OnCheatTyped;
}