using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EventDialog : UIScreen
{
    // TODO: MapGenerator has Nodes which have Events. Use those to display correct dialog.

    public Text description;
    public EventButton[] buttons;
    private EventType actionEvent;

    public void Init()
    {
        buttons = GetComponentsInChildren<EventButton>();
        foreach (EventButton button in buttons)
        {
            button.Init();
        }
    }

    public void ShowEvent(EventType actionEvent, string gain)
    {
        if (actionEvent != null)
        {
            this.actionEvent = actionEvent;
            if (gain != null && gain.Length > 0)
            {
                description.text = string.Format(actionEvent.Description, gain);
            }
            else
            {
                description.text = actionEvent.Description;
            }
            DisplayButtons();
            Activate(true);
        }
        else
        {
            Debug.LogError("Can't show event, it is null.");
        }
    }

    public void ShowResults(string resultText, string gain, string confirmText)
    {
        if (actionEvent != null)
        {
            if (gain != null && gain.Length > 0)
            {
                description.text = string.Format(resultText, gain);
            }
            else
            {
                description.text = resultText;
            }

            DisplayConfirmButton(confirmText);
            Activate(true);
        }
        else
        {
            Debug.LogError("Can't show results, event is null.");
        }
    }

    private void DisplayButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < actionEvent.Choices.Count)
            {
                buttons[i].InitEvent(actionEvent, i);
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
