using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    public EventDialog eventDialog;
    public GameOverScreen gameOverScreen;
    public GameObject battleSkills;
    public GameObject progressPanel;
    public Text regionText;
    public Text scoreText;

    private EventManager eventManager;

    // Start is called before the first frame update
    private void Start()
    {
        eventDialog.Init();
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO
    }

    public void SetEventManager(EventManager eventManager)
    {
        this.eventManager = eventManager;
    }

    public void OnSceneChanged(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.MainMenu:
            {
                progressPanel.SetActive(false);
                break;
            }
            case GameManager.GameState.Map:
            {
                UpdateRegion(GameManager.Instance.regionNum);
                progressPanel.SetActive(true);
                break;
            }
            case GameManager.GameState.Battle:
            {
                progressPanel.SetActive(false);
                break;
            }
        }

        UpdateScore(GameManager.Instance.GetScore());
    }

    public void ActivateBattleSkills(bool active)
    {
        battleSkills.SetActive(active);
    }

    public void CloseDialogs()
    {
        eventManager.EndEventWithoutResults();
    }

    public void UpdateRegion(int region)
    {
        regionText.text = string.Format("Area: {0}", region);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = string.Format("Score: {0}", score);
    }

    public void EndGame(bool win)
    {
        gameOverScreen.EndGame(win);
    }

    public void CloseScreens()
    {
        mainMenu.Activate(false);
        pauseMenu.Activate(false);
        CloseDialogs();
        ActivateBattleSkills(false);
        gameOverScreen.Activate(false);
    }
}
