using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using data;

public class EventButton : MonoBehaviour
{
    private Action action;
    private Text text;

    public void Init()
    {
        text = GetComponentInChildren<Text>();
    }

    public void InitEvent(string title, Action action)
    {
        text.text = title;
        this.action = action;
    }

    public void InitResults(string title)
    {
        text.text = title;
    }

    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

    public void DoAction()
    {
        if (!GameManager.Instance.eventManager.eventResults)
        {
            switch (action)
            {
                case Action.Fight:
                {
                    // TODO: Start fighting.
                    break;
                }
                case Action.Loot:
                {
                    // TODO: Get loot.
                    break;
                }
                case Action.Advance:
                {
                    // TODO: Load next map.
                    GameManager.Instance.EndGame(true); // testing
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        GameManager.Instance.eventManager.EndEvent(false);
    }
}
