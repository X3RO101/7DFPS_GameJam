using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    private bool guide = false;
    [SerializeField] private CanvasGroup guideCG = null;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void LateUpdate()
    {
        if(guide)
        {
            if (Input.anyKey)
            {
                LoadTutorial();
            }

        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void LoadTutorial()
    {
        guide = !guide;

        if(guide)
        {
            guideCG.DOFade(1f, 0.25f);
        }
        else
        {
            guideCG.DOFade(0f, 0.25f);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
