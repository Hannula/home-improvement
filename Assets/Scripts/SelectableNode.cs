using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

public class SelectableNode : SelectableItem
{
    public Node node;

    private EventManager eventManager;
    private MapManager mapManager;

    private void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
        mapManager = FindObjectOfType<MapManager>();
    }

    // Everytime item is clicked
    public override void OnClick()
    {
        
    }

    // Only when selected the first time
    public override void OnSelect()
    {
        if (!mapManager.HomeMoving && node != null && mapManager.allowedNodesToMoveTo().Contains(node))
        {
            mapManager.MoveToNode(node);
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
