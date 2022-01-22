using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickEvents : MonoBehaviour
{
    public void GotoLevel()
    {
        SceneManager.LoadScene("Testlevel", LoadSceneMode.Single);
    }

    public void GotoSettings()
    {
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Single);
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void GotoExitGame()
    {
        Debug.Log("Bye!");
        Application.Quit();
    }
}
