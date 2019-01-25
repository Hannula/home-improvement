using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : UIScreen
{
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;

    // Start is called before the first frame update
    private void Start()
    {
        // TODO
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO
    }

    public void ResumeGame()
    {
        GameManager.Instance.PauseGame(false);
    }

    public void RestartGame()
    {
        GameManager.Instance.LoadNewGame();
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
