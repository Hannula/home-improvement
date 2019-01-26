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

    // Start is called before the first frame update
    private void Start()
    {
        buttons = GetComponentsInChildren<EventButton>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void ShowDialog(Event actionEvent)
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
            Debug.LogError("Event is null.");
        }
    }

    private void DisplayButtons()
    {
        List<string> buttonNames = actionEvent.Choises.Keys.ToList();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < buttonNames.Count)
            {
                buttons[i].title = buttonNames[i];
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
