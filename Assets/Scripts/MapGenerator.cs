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
        public Event Event = new Event();
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
        Loot,
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

    public int NodeCount = 10;
    public int totalCountOfAreas = 0;

    private System.Random rand = new System.Random();

    public void Generate()
    {
        // Remove start and end
        var nodesInTheMiddle = NodeCount - 2;
        if (nodesInTheMiddle <= 2)
        {
            totalCountOfAreas = 3;
        }
        else if (nodesInTheMiddle > 2 && nodesInTheMiddle <= 5)
        {
            totalCountOfAreas = 4;
        }
        else if (nodesInTheMiddle > 5 && nodesInTheMiddle <= 9)
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
        var choices = new Dictionary<string, Action>() { { "OK", Action.None } };
        home.Event = new Event() { Description = "You returned home", Choises = choices };
        AllNodes.Add(home);
        HomeNode = home;

        var goal = new Node();
        goal.Area = totalCountOfAreas;
        goal.id = NodeCount;
        addToAreaToNodeMapping(goal.Area, goal);
        choices = new Dictionary<string, Action>() { { "OK", Action.Advance } };
        home.Event = new Event() { Description = "You can continue to next area", Choises = choices };
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

            if (AreaToNodeMapping[randomNumber].Count() < 3)
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
        var randomNumber = rand.Next(2);
        Dictionary<string, Action> choices = new Dictionary<string, Action>();
        if (randomNumber == 0)
        {
            choices.Add("Yes", Action.Loot);
            choices.Add("No", Action.None);
            node.Event = new Event(){ Description = "Do you want to loot old castle?", Choises = choices };
        }
        else
        {
            choices.Add("FIGHT", Action.Fight);
            node.Event = new Event() { Description = "There Slimy Church!", Choises = choices };
        }

        return node;
    }
}


