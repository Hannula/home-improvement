using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    public EventDialog eventDialog;
    public GameOverScreen gameOverScreen;

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

    public void ShowEventDialog(data.Event actionEvent)
    {
        eventDialog.ShowEvent(actionEvent);
    }

    public void ShowResultDialog()
    {
        string confirmText = "OK";
        eventDialog.ShowResults(confirmText);
    }

    public void CloseDialogs()
    {
        eventManager.EndEvent(true);
        //resultDialog.Close();
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
        gameOverScreen.Activate(false);
    }
}
