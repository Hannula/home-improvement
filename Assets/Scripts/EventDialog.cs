using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using data;

public class EventDialog : UIScreen
{
    // TODO: MapGenerator has Nodes which have Events. Use those to display correct dialog.

    public Text description;
    public EventButton[] buttons;
    private data.Event actionEvent;

    // Start is called before the first frame update
    private void Start()
    {
        buttons = GetComponentsInChildren<EventButton>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SetupDialog(data.Event actionEvent)
    {
        //Activate(true);
        this.actionEvent = actionEvent;
        description.text = actionEvent.Description;
        DisplayButtons();
    }

    public void DisplayButtons()
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
}
