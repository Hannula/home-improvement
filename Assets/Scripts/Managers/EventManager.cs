using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// The currently occupied node.
    /// </summary>
    public Node currentNode;

    /// <summary>
    /// Is an event active.
    /// </summary>
    public bool eventActive;

    /// <summary>
    /// Is an event ending.
    /// </summary>
    public bool eventResults;

    private UIManager ui;
    private SelectableNode[] nodes;

    // Start is called before the first frame update
    private void Start()
    {
        ui = FindObjectOfType<UIManager>();
        ui.SetEventManager(this);
        nodes = FindObjectsOfType<SelectableNode>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void StartEvent(Node node)
    {
        if (GameManager.Instance.State == GameManager.GameState.Map)
        {
            if (!eventActive)
            {
                eventActive = true;
                currentNode = node;
                ui.ShowEventDialog(node.Event);
                SFXPlayer.Instance.Play(Sound.GetRekt2); // testing
            }
        }
        else
        {
            Debug.LogError("Events can only happen in the Map screen.");
        }
    }

    private void EventResults()
    {
        if (GameManager.Instance.State == GameManager.GameState.Map)
        {
            ui.ShowResultDialog();
        }
        else
        {
            Debug.LogError("Event results can only be viewed in the Map screen.");
        }

    }

    public void EndEvent(bool skipConfirm)
    {
        if (eventActive)
        {
            if (eventResults || skipConfirm)
            {
                eventActive = false;
                eventResults = false;
                ui.eventDialog.Close();
            }
            else
            {
                eventResults = true;
                EventResults();
            }
        }
    }
}
