using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        //Configuracion global de DOTween
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
