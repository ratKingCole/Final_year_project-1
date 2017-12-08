using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {
    public Button btnRes1;
    public Button btnRes2;
    public Button btnRes3;
    public Button btnBack;

    public Button btnPlay;
    public Button btnSettings;
    public Button btnExit;

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
        btnPlay.gameObject.SetActive(true);
        btnSettings.gameObject.SetActive(true);
        btnExit.gameObject.SetActive(true);

        btnRes1.gameObject.SetActive(false);
        btnRes2.gameObject.SetActive(false);
        btnRes3.gameObject.SetActive(false);
        btnBack.gameObject.SetActive(false);
    }

    public void SettingsButton()
    {
        btnPlay.gameObject.SetActive(false);
        btnSettings.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);

        btnRes1.gameObject.SetActive(true);
        btnRes2.gameObject.SetActive(true);
        btnRes3.gameObject.SetActive(true);
        btnBack.gameObject.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
