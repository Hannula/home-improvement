using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    public EventDialog eventDialog;
    public GameOverScreen gameOverScreen;
    public GameObject battleSkills;

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

    public void ActivateBattleSkills(bool active)
    {
        battleSkills.SetActive(active);
    }

    public void CloseDialogs()
    {
        eventManager.EndEventWithoutResults();
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
