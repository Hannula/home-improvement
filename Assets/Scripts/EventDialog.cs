using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDialog : UIScreen
{
    public EventButton[] buttons;

    // Start is called before the first frame update
    private void Start()
    {
        buttons = GetComponentsInChildren<EventButton>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void DisplayButtons(string[] buttonNames)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < buttonNames.Length)
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
