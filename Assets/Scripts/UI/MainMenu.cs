using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIScreen
{
    public Button startButton;
    public Button quitButton;

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

    public void StartGame()
    {
        GameManager.Instance.LoadNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
