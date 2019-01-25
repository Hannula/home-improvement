using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    public EventDialog eventDialog;

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

    public void ShowEventDialog()
    {
        // TODO: Event class with event info.
        // Display the event.
    }

    public void ShowResultDialog()
    {
        // TODO: Result class with result info.
        // Display the result by using the EventDialog
    }

    public void CloseMenus()
    {
        mainMenu.Activate(false);
        pauseMenu.Activate(false);
    }
}
