using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIScreen
{
    public Button startButton;
    public Button instructionsButton;
    public Button quitButton;
    public GameObject instructionsScreen;

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

    public void QuitGame()
    {
        Application.Quit();
    }
}
