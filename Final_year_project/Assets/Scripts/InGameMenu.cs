using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour {
    public Button btnResume;
    public Button btnSettings;
    public Button btnExitGame;
    public Button btnExitMenu;

    public Button btnRes1;
    public Button btnRes2;
    public Button btnRes3;
    public Button btnBack;
    bool menuShowing = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuShowing == false)
            {
                btnResume.gameObject.SetActive(true);
                btnSettings.gameObject.SetActive(true);
                btnExitMenu.gameObject.SetActive(true);
                btnExitGame.gameObject.SetActive(true);

                menuShowing = true;
            }
            else
            {
                btnResume.gameObject.SetActive(false);
                btnSettings.gameObject.SetActive(false);
                btnExitMenu.gameObject.SetActive(false);
                btnExitGame.gameObject.SetActive(false);
                btnRes1.gameObject.SetActive(false);
                btnRes2.gameObject.SetActive(false);
                btnRes3.gameObject.SetActive(false);
                btnBack.gameObject.SetActive(false);

                menuShowing = false;
            }
        }
    }

    public void ChangeRes(int resolution)
    {
        switch (resolution)
        {
            case 1:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 2:
                Screen.SetResolution(1600, 900, true);
                break;
            case 3:
                Screen.SetResolution(1280, 720, true);
                break;

            default:
                break;
        }
    }

    public void BackButton()
    {
        btnResume.gameObject.SetActive(true);
        btnSettings.gameObject.SetActive(true);
        btnExitMenu.gameObject.SetActive(true);
        btnExitGame.gameObject.SetActive(true);


        btnRes1.gameObject.SetActive(false);
        btnRes2.gameObject.SetActive(false);
        btnRes3.gameObject.SetActive(false);
        btnBack.gameObject.SetActive(false);
    }

    public void SettingsButton()
    {
        btnResume.gameObject.SetActive(false);
        btnSettings.gameObject.SetActive(false);
        btnExitMenu.gameObject.SetActive(false);
        btnExitGame.gameObject.SetActive(false);

        btnRes1.gameObject.SetActive(true);
        btnRes2.gameObject.SetActive(true);
        btnRes3.gameObject.SetActive(true);
        btnBack.gameObject.SetActive(true);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void ExitMenuButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResumeButton()
    {
        btnResume.gameObject.SetActive(false);
        btnSettings.gameObject.SetActive(false);
        btnExitMenu.gameObject.SetActive(false);
        btnExitGame.gameObject.SetActive(false);

        menuShowing = false;
    }
}