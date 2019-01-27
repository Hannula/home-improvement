using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using data;

public class EventButton : MonoBehaviour
{
    private EventType actionEvent;
    private Text text;
    private int actionIndex;
    private bool confirmResults;

    public void Init()
    {
        text = GetComponentInChildren<Text>();
    }

    public void InitEvent(EventType actionEvent, int index)
    {
        this.actionEvent = actionEvent;
        text.text = actionEvent.Choices[index].Name;
        actionIndex = index;
        confirmResults = false;
    }

    public void InitResults(string title)
    {
        confirmResults = true;
        text.text = title;
    }

    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

    public void DoAction()
    {
        if (confirmResults)
        {
            GameManager.Instance.eventManager.EndEventWithoutResults();
        }
        else
        {
            GameManager.Instance.eventManager.EndEvent(actionEvent, actionIndex);
        }
    }
}
