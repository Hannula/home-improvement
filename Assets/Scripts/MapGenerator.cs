using System;
using System.Collections.Generic;
using System.Linq;

public class MapGenerator
{
    public List<Node> AllNodes;
    public Node HomeNode;
    public Node GoalNode;

    public int NodeCount = 10;

    private Random rand;

    public MapGenerator()
    {
        var nodesInTheMiddle = NodeCount - 2;
        int totalCountOfAreas;
        if (nodesInTheMiddle <= 2)
        {
            totalCountOfAreas = 3;
        }
        else if (nodesInTheMiddle > 2 && nodesInTheMiddle <= 5)
        {
            totalCountOfAreas = 4;
        }
        else if (nodesInTheMiddle < 5 && nodesInTheMiddle <= 9)
        {
            totalCountOfAreas = 5;
        }
        else
        {
            throw new System.Exception("Too many nodes, Think more Count of areas in MapGenerator");
        }

        var home = new Node();
        home.Area = 1;
        var choices = new Dictionary<string, Action>() { { "OK", Action.None } };
        home.Event = new Event() { Description = "You returned home", Choises = choices };
        AllNodes.Add(home);

        var goal = new Node();
        goal.Area = totalCountOfAreas;
        choices = new Dictionary<string, Action>() { { "OK", Action.Advance } };
        home.Event = new Event() { Description = "You can continue to next area", Choises = choices };
        AllNodes.Add(goal);

        // Create rest of the nodes
        var areaIter = 2;
        var lastNode = HomeNode;
        while (areaIter != totalCountOfAreas)
        {
            var node = GenerateRandomNode();
            node.Area = areaIter;
            lastNode.Neighbours.Add(node);
            node.Neighbours.Add(lastNode);
            areaIter++;
            AllNodes.Add(node);

            // Last one, combine with goal node
            if (areaIter + 1 == totalCountOfAreas)
            {
                node.Neighbours.Add(goal);
                goal.Neighbours.Add(node);
            }
        }

        while (AllNodes.Count != NodeCount)
        {
            var areaCounts = AllNodes.Select(s => s).ToList();
            var randomNumber = rand.Next(2, totalCountOfAreas);

            if (areaCounts.Where(x => x.Area.Equals(randomNumber)).Count() < 3)
            {
                // Find node in area left to this node and choose random of them to connect to
                var nodesInLeft = areaCounts.Where(x => x.Equals(randomNumber - 1));
                int toSkip = rand.Next(0, nodesInLeft.Count());
                var connectToLeft = nodesInLeft.Skip(toSkip).Take(1).First();

                var nodesInRight = areaCounts.Where(x => x.Equals(randomNumber - 1));
                toSkip = rand.Next(0, nodesInRight.Count());
                var connectToRight = nodesInLeft.Skip(toSkip).Take(1).First();

                var node = GenerateRandomNode();
                node.Area = randomNumber;

                node.Neighbours.Add(connectToLeft);
                connectToLeft.Neighbours.Add(node);

                node.Neighbours.Add(connectToRight);
                connectToRight.Neighbours.Add(node);

                AllNodes.Add(node);
            }
        }
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


public class Node
{
    public List<Node> Neighbours;
    public int Area;
    public bool Exit = false;
    public Event Event;
}

public class Event
{
    public string Description = "You encountered enemy!";
    public Dictionary<string, Action> Choises;
}

public enum Action
{
    Fight,
    Loot,
    Advance,
    None
}