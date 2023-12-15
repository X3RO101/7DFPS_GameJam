using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void LoadTutorial()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
