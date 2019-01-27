using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

public class SaveDataPackage
{
    public List<data.Node> Nodes = new List<Node>();
    public List<GameObject> NodeGameObjects = new List<GameObject>();
    public List<data.Node> LootedNodes = new List<Node>();
    public List<data.Node> DangeredNodes = new List<Node>();
    public Node CurrentNode;
    public int DangerZoneAdvance;
}
