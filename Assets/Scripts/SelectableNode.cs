using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

public class SelectableNode : SelectableItem
{
    public Node node;

    private EventManager eventManager;

    private void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    // Everytime item is clicked
    public override void OnClick()
    {
        
    }

    // Only when selected the first time
    public override void OnSelect()
    {
        if (node != null)
        {
            eventManager.StartEvent(node);
        }
        else
        {
            Debug.LogError("This node does not contain any data.");
        }
    }

    public override void OnUnselect()
    {

    }

    public void setNode(Node node)
    {
        this.node = node;
    }
}
