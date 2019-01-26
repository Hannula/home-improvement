using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    public EventDialog eventDialog;
    public GameOverScreen gameOverScreen;

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

    public void ShowEventDialog(data.Event actionEvent)
    {
        eventDialog.ShowDialog(actionEvent);
        eventDialog.Activate(true);
    }

    public void ShowResultDialog()
    {
        // TODO: Result class with result info.
        // Display the result by using the EventDialog
    }

    public void CloseDialogs()
    {
        eventDialog.Close();
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
