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

		AudioManager.inst.Play("mainmenubgm");
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
        AudioManager.inst.StopAllSounds();
        AudioManager.inst.Play("gameplaybgm");
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

    public void OnButtonHover()
    {
        AudioManager.inst.Play("mmbtnhover");
    }
    public void OnButtonClick()
    {
        AudioManager.inst.Play("mmbtnclick");
    }
}
