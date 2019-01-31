using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : UIScreen
{
    public Button restartButton;
    public Button mainMenuButton;
    public Text gameOverText;

    private string winText = "Congratulations! You win!";
    private string loseText = "You lost! Score: {0}";

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

    public void EndGame(bool win)
    {
        if (win)
        {
            gameOverText.text = winText;
        }
        else
        {
            gameOverText.text = string.Format(loseText, GameManager.Instance.GetScore());
        }
        
        Activate(true);
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
