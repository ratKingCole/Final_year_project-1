using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject PlayMenu;
    public GameObject SettingsMenu;
    public GameObject ResolutionMenu;
    public GameObject ControlsMenu;

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

    public void TurnOnControlsMenu()
    {
        ControlsMenu.SetActive(true);
    }

    public void TurnOffControlsMenu()
    {
        ControlsMenu.SetActive(false);
    }

    public void TurnOnResoluionMenu()
    {
        ResolutionMenu.SetActive(true);
    }

    public void TurnOffResolutionMenu()
    {
        ResolutionMenu.SetActive(false);
    }
}
