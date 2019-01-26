using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

public class EventManager : MonoBehaviour
{
    public Node currentNode;
    public bool eventActive;

    private UIManager ui;
    private SelectableNode[] nodes;

    // Start is called before the first frame update
    private void Start()
    {
        ui = FindObjectOfType<UIManager>();
        nodes = FindObjectsOfType<SelectableNode>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void StartEvent(Node node)
    {
        currentNode = node;
        if (GameManager.Instance.State == GameManager.GameState.Map)
        {
            ui.ShowEventDialog(node.Event);
        }
        else
        {
            Debug.LogError("Events can only happen in the Map screen.");
        }
    }
}
