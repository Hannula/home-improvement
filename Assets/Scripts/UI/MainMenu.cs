using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIScreen
{
    public Button startButton;
    public Button instructionsButton;
    public Button creditsButton;
    public Button quitButton;
    public GameObject instructionsScreen;
    public GameObject creditsScreen;

    public void StartGame()
    {
        GameManager.Instance.LoadNewGame();
    }

    public void OpenInstructions()
    {
        instructionsScreen.SetActive(true);
    }

    public void CloseInstructions()
    {
        instructionsScreen.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsScreen.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
