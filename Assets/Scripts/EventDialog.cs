using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using data;

using Event = data.Event;

public class EventDialog : UIScreen
{
    // TODO: MapGenerator has Nodes which have Events. Use those to display correct dialog.

    public Text description;
    public EventButton[] buttons;
    private Event actionEvent;

    public void Init()
    {
        buttons = GetComponentsInChildren<EventButton>();
        foreach (EventButton button in buttons)
        {
            button.Init();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void ShowEvent(Event actionEvent)
    {
        if (actionEvent != null)
        {
            this.actionEvent = actionEvent;
            description.text = actionEvent.Description;
            DisplayButtons();
            Activate(true);
        }
        else
        {
            Debug.LogError("Can't show event, it is null.");
        }
    }

    public void ShowResults(string confirmText)
    {
        if (actionEvent != null)
        {
            description.text = actionEvent.Description;
            DisplayConfirmButton(confirmText);
            Activate(true);
        }
        else
        {
            Debug.LogError("Can't show results, Event is null.");
        }
    }

    private void DisplayButtons()
    {
        List<string> buttonNames = actionEvent.Choises.Keys.ToList();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < buttonNames.Count)
            {
                buttons[i].InitEvent(buttonNames[i], actionEvent.Choises[buttonNames[i]]);
                buttons[i].Activate(true);
            }
            else
            {
                buttons[i].Activate(false);
            }
        }
    }

    private void DisplayConfirmButton(string buttonText)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == 0)
            {
                buttons[i].InitResults(buttonText);
                buttons[i].Activate(true);
            }
            else
            {
                buttons[i].Activate(false);
            }
        }
    }

    public void Close()
    {
        actionEvent = null;
        Activate(false);
    }
}
