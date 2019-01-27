using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using data;

using Action = data.Action;
using Event = data.Event;
namespace data
{
    public class Node
    {
        public int id;
        public List<Node> Neighbours = new List<Node>();
        public int Area;
        public bool Exit = false;
        public EventType Event = new EventType();
        public Vector3 Position = new Vector3();
    }

    public class Event
    {
        public string Description = "[DEFAULT]";
        public Dictionary<string, Action> Choises = new Dictionary<string, Action>();
    }

    public enum Action
    {
        Fight,
        Gain,
        Lose,
        Advance,
        None
    }
}

public class MapGenerator
{
    public List<Node> AllNodes = new List<Node>();
    public Dictionary<int, List<Node>> AreaToNodeMapping = new Dictionary<int, List<Node>>();
    public Node HomeNode;
    public Node GoalNode;

    public int NodeCount;
    public int totalCountOfAreas = 0;

    private System.Random rand = new System.Random();

    public void Generate()
    {
        NodeCount = rand.Next(8, 16);
        // Remove start and end
        var nodesInTheMiddle = NodeCount - 2;
        if (nodesInTheMiddle <= 2)
        {
            totalCountOfAreas = 3;
        }
        else if (nodesInTheMiddle > 2 && nodesInTheMiddle <= 6)
        {
            totalCountOfAreas = 4;
        }
        else if (nodesInTheMiddle > 6 && nodesInTheMiddle <= 16)
        {
            totalCountOfAreas = 5;
        }
        else
        {
            throw new System.Exception($"Too many nodes: {nodesInTheMiddle}, Think more Count of areas in MapGenerator");
        }

        var nodeIDCounter = 1;

        var home = new Node();
        home.Area = 1;
        home.id = nodeIDCounter++;
        addToAreaToNodeMapping(1, home);
        home.Event = ContentManager_Events.Instance.GetHomeEvent();
        AllNodes.Add(home);
        HomeNode = home;

        var goal = new Node();
        goal.Area = totalCountOfAreas;
        goal.id = NodeCount;
        addToAreaToNodeMapping(goal.Area, goal);
        goal.Event = ContentManager_Events.Instance.GetGoalEvent();
        AllNodes.Add(goal);
        GoalNode = goal;

        // Create rest of the nodes
        var areaIter = 2;
        var lastNode = HomeNode;
        while (areaIter != totalCountOfAreas)
        {
            var node = GenerateRandomNode();
            node.Area = areaIter;
            node.id = nodeIDCounter++;
            addToAreaToNodeMapping(node.Area, node);
            lastNode.Neighbours.Add(node);
            node.Neighbours.Add(lastNode);
            AllNodes.Add(node);
            // Debug.Log(string.Format("This node ID: {0}, Area: {1}, Connecting from left to id: {2}, Area: {3} ", node.id, node.Area, lastNode.id, lastNode.Area));

            lastNode = node;

            // Debug.Log("Adding node to area:" + node.Area);
            // Last one, combine with goal node
            if (areaIter + 1 == totalCountOfAreas)
            {
                node.Neighbours.Add(goal);
                goal.Neighbours.Add(node);
            }

            areaIter++;
        }

        while (AllNodes.Count != NodeCount)
        {
            var areaCounts = AllNodes.Select(s => s).ToList();
            var randomNumber = rand.Next(2, totalCountOfAreas);

            if (AreaToNodeMapping[randomNumber].Count() < 5)
            {
                // Find node in area left to this node and choose random of them to connect to
                var nodesInLeft = AreaToNodeMapping[randomNumber - 1];
                int toSkip = rand.Next(0, nodesInLeft.Count());
                var connectToLeft = nodesInLeft.Skip(toSkip).Take(1).First();

                var nodesInRight = AreaToNodeMapping[randomNumber + 1];
                toSkip = rand.Next(0, nodesInRight.Count());
                var connectToRight = nodesInRight.Skip(toSkip).Take(1).First();


                var node = GenerateRandomNode();
                node.id = nodeIDCounter++;
                node.Area = randomNumber;
                addToAreaToNodeMapping(node.Area, node);

                node.Neighbours.Add(connectToLeft);
                connectToLeft.Neighbours.Add(node);

                node.Neighbours.Add(connectToRight);
                connectToRight.Neighbours.Add(node);

                // Debug.Log(string.Format("This node ID: {0}, Area: {1}, Connecting from left to id: {2}, Area: {3} And connecting from right id: {4}, Area: {5}", node.id, node.Area, connectToLeft.id, connectToLeft.Area, connectToRight.id, connectToRight.Area));

                AllNodes.Add(node);
            }
        }
    }

    private void addToAreaToNodeMapping(int area, Node node)
    {
        List<Node> list = new List<Node>();
        if (!AreaToNodeMapping.TryGetValue(area, out list))
        {
            list = new List<Node>();
            AreaToNodeMapping.Add(area, list);
        }

        list.Add(node);
    }


    public Node GenerateRandomNode()
    {
        var node = new Node();
        var randomNumber = 1 + rand.Next(2);
        node.Event = ContentManager_Events.Instance.GetRandomEvent(randomNumber);

        return node;
    }




    public void InitializeWithLoadedData(List<Node> nodes)
    {
        AllNodes = nodes;
        foreach (var node in nodes)
        {
            addToAreaToNodeMapping(node.Area, node);
        }
        totalCountOfAreas = nodes.Select(s => s.Area).Distinct().Count();
        HomeNode = nodes.Where(n => n.id == 1).First();
        GoalNode = nodes.Where(n => n.id == nodes.Count()).First();
        NodeCount = nodes.Count;
    }
}


