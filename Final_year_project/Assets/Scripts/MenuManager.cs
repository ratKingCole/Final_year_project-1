using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject PlayMenu;
    public GameObject SettingsMenu;

    public void TurnOnMainMenu()
    {
        MainMenu.SetActive(true);
    }

    public void TurnOffMainMenu()
    {
        MainMenu.SetActive(false);
    }

    public void TurnOnPlayMenu()
    {
        PlayMenu.SetActive(true);
    }

    public void TurnOffPlayMenu()
    {
        PlayMenu.SetActive(false);
    }

    public void TurnOnSettingsMenu()
    {
        SettingsMenu.SetActive(true);
    }

    public void TurnOffSettingsMenu()
    {
        SettingsMenu.SetActive(false);
    }
}
