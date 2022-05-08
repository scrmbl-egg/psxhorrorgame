using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{



    private void Awake()
    {
        //animations capacity in RAM memory.


        //TODO: This doesn't belong in the game manager
        DOTween.Init(true, true, LogBehaviour.Default).SetCapacity(200, 10);
    }
}
