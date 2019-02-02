using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using data;
using static Assets.Scripts.LineDrawer;
using Assets.Scripts;

public class MapManager : MonoBehaviour
{
    private readonly int screenWidth = 640;
    private readonly System.Random rand = new System.Random();

    public MapGenerator mapGenerator = new MapGenerator();
    public LineDrawer lineDrawer;

    public List<data.Node> Nodes = new List<Node>();
    public List<GameObject> NodeGameObjects = new List<GameObject>();
    public List<data.Node> LootedNodes = new List<Node>();
    public List<data.Node> DangeredNodes = new List<Node>();

    public Dictionary<data.Node, GameObject> NodeToGameobjectMapping = new Dictionary<Node, GameObject>();
    public Dictionary<GameObject, data.Node> GameobjectToNodeMapping = new Dictionary< GameObject, data.Node>();

    public int mapHeight;
    public int mapWidth;

    public Transform nodePrefab;

    public Material lineMaterial;

    public Transform Dangerzone;

    public Node currentNode;

    public Transform HomeIconPrefab;
    public GameObject HomeIcon;
    public Sprite HomeIconDrivingSprite;
    private Sprite HomeIconIdleSprite;

    private Transform homeTarget;
    public bool HomeMoving = false;
    private EventManager eventManager;
    private GameObject homeHolder;
    private GameObject lineHolder;
    private bool loadedSaveData;

    void Start()
    {
        lineDrawer = new LineDrawer(lineMaterial);
        loadedSaveData = false;
        homeHolder = new GameObject();
        homeHolder.name = "HomeHolder";
        lineHolder = new GameObject();
        lineHolder.name = "LineHolder";
        eventManager = FindObjectOfType<EventManager>();

        var mapTexture = GameObject.Find("MapArea").GetComponent<SpriteRenderer>().sprite.texture;
        var homeAreaTexture = GameObject.Find("HomePanel").GetComponent<SpriteRenderer>().size.x;
        mapWidth = mapTexture.width;
        mapHeight = mapTexture.height;

        var homeAreaWidthPixels = (int)Math.Round((homeAreaTexture * 32));
        mapWidth = screenWidth - homeAreaWidthPixels;

        var savedData = GameManager.Instance.LoadMapState();
        if (savedData.Nodes.Count() != 0)
        {
            mapGenerator.InitializeWithLoadedData(savedData.Nodes);
            Nodes = savedData.Nodes;
            advance = savedData.DangerZoneAdvance;
            LootedNodes = savedData.LootedNodes;
            DangeredNodes = savedData.DangeredNodes;
            loadedSaveData = true;
            currentNode = savedData.CurrentNode;
            // NodeGameObjects = savedData.NodeGameObjects;
        }
        else
        {
            mapGenerator.Generate();
            Nodes = mapGenerator.AllNodes;
        }

        int areaWidth = mapWidth / mapGenerator.totalCountOfAreas;

        for (int i = 0; i != mapGenerator.totalCountOfAreas; i++)
        {
            var nodesInThisArea = mapGenerator.AreaToNodeMapping[i + 1];

            var areaNodeCounter = 0;
            foreach (var node in nodesInThisArea)
            {
                areaNodeCounter += 1;
                var offset = rand.Next(16, 24);
                offset = rand.Next(1) == 1 ? offset : -offset;
                var go = GameObject.Find("MapArea").transform;

                float xPosition = i * areaWidth + areaWidth / 2 + offset + 20;

                offset = rand.Next(16, 32);
                offset = rand.Next(1) == 1 ? offset : -offset;
                float yPosition = mapHeight / nodesInThisArea.Count() / 2 * areaNodeCounter + offset;


                xPosition = xPosition - mapWidth/2 + homeAreaWidthPixels/2;
                yPosition = yPosition - mapHeight/2/2;

                // Pixel space to screen space
                xPosition = xPosition / 32;
                yPosition = yPosition / 32;
                node.Position = new Vector3(xPosition, yPosition, 0);

                var homeGo = Instantiate(nodePrefab, node.Position, Quaternion.identity);
                NodeGameObjects.Add(homeGo.gameObject);
                NodeToGameobjectMapping.Add(node, homeGo.gameObject);
                GameobjectToNodeMapping.Add(homeGo.gameObject, node);
                homeGo.gameObject.GetComponent<SelectableNode>().setNode(node);
                homeGo.SetParent(homeHolder.transform);
                homeGo.name = "HomeNode";
            }
        }

        if(!loadedSaveData)
        {
            currentNode = mapGenerator.HomeNode;
        }
        
        drawNodeCircles();

        var homeObj = Instantiate(HomeIconPrefab, currentNode.Position, Quaternion.identity);
        HomeIcon = homeObj.gameObject;
        homeTarget = homeObj.transform;

        if (loadedSaveData)
        {
            advance -= 1;
            AdvanceDangerZone();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ActiveGame && HomeMoving)
        {
            if (Vector3.Distance(HomeIcon.transform.position, homeTarget.transform.position) > 0.05f)
            {
                HomeIcon.transform.position += (homeTarget.transform.position - HomeIcon.transform.position).normalized * 2 * Time.deltaTime;
            }
            else
            {
                HomeMoving = false;
                ArrivedToNode(homeTarget);
            }
        }
    }


    

    public List<Node> allowedNodesToMoveTo()
    {
        var list = new List<Node>();
        list.AddRange(currentNode.Neighbours);

        foreach (var node in DangeredNodes)
        {
            if (list.Contains(node))
            {
                list.Remove(node);
            }
        }
        return list;
    }

    private Transform dangerZoneGameObject;
    private Transform dangerZoneTarget;
    private int advance = 0;
    private List<GameObject> dangerCircle = new List<GameObject>();
    public void AdvanceDangerZone()
    {
        // Debug.Log("Advancing!");
        advance = advance + 1;
        if (advance == 1 || loadedSaveData)
        {
            dangerZoneGameObject = Instantiate(Dangerzone, new Vector3(-200/32, 0, 0), Quaternion.identity);
            dangerZoneTarget = dangerZoneGameObject.transform;

            //dangerZoneGameObject.transform.position = new Vector3(-400 / 32, -20 /32, 0);
        }

        if (advance == 2)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[1]);
            dangerZoneGameObject.transform.position = new Vector3(-150 / 32, 0, 0);
        }
        else if (advance == 4)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[2]);
            dangerZoneGameObject.transform.position = new Vector3(-100 / 32, 0, 0);
        }
        else if (advance == 6)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[3]);
            dangerZoneGameObject.transform.position = new Vector3(-50 / 32, 0, 0);
        }
        else if (advance == 8)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[4]);
            dangerZoneGameObject.transform.position = new Vector3(0, 0, 0);
        }
        else if (advance == 10)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[5]);
            dangerZoneGameObject.transform.position = new Vector3(50 / 32, 0, 0);
        }
        else if (advance == 12)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[6]);
            dangerZoneGameObject.transform.position = new Vector3(100 / 32, 0, 0);
        }

        foreach (var s in dangerCircle)
        {
            Destroy(s);
        }

        //dangerCircle = drawCircle(dangerZoneGameObject, 40, 8f + 1.5f * advance, 0.6f, Color.yellow);
    }


    public void MoveToNode(Node node)
    {
        LootedNodes.Add(currentNode);
        currentNode = node;
        homeTarget = NodeToGameobjectMapping[node].transform;
        drawNodeCircles();
        var sprite = HomeIcon.GetComponent<SpriteRenderer>().sprite;
        HomeIconIdleSprite = sprite;
        sprite = HomeIconDrivingSprite;
        HomeMoving = true;
    }

    public void ArrivedToNode(Transform t)
    {
        AdvanceDangerZone();
        HomeIcon.GetComponent<SpriteRenderer>().sprite = HomeIconIdleSprite;
        GameManager.Instance.SaveMapState(new SaveDataPackage()
        {
            CurrentNode = currentNode,
            DangeredNodes = DangeredNodes,
            LootedNodes = LootedNodes,
            NodeGameObjects = NodeGameObjects,
            DangerZoneAdvance = advance,
            Nodes = Nodes
        });
        eventManager.StartEvent(NodeToGameobjectMapping.Where(kvp => kvp.Value == t.gameObject).Select(kvp => kvp.Key).First());
    }

    private List<GameObject> nodeLines = new List<GameObject>();
    private void drawNodeCircles()
    {
        foreach (var line in nodeLines)
        {
            Destroy(line);
        }

        // Draw line from node to node. Skip if already drawn from other direction
        foreach (var kvp in NodeToGameobjectMapping)
        {
            foreach (var neighbour in kvp.Key.Neighbours)
            {
                // TODO dont draw twice by mapping what pairs have already been drawn. 
                // Really unnecessary improvement at the moment though
                var allowed = allowedNodesToMoveTo().Contains(kvp.Key) && (currentNode == kvp.Key || currentNode == neighbour);
                nodeLines.Add(lineDrawer.DrawLine(kvp.Value, neighbour.Position, lineHolder, allowed));
            }
        }

        // Draw circle for node
        foreach (var kvp in NodeToGameobjectMapping)
        {
            var type = getNodeType(kvp.Value);
            nodeLines.AddRange(lineDrawer.drawNodeCircle(kvp.Value, type, LootedNodes.Contains(kvp.Key), lineMaterial));
        }
    }

    private NodeType getNodeType(GameObject go)
    {
        var node = new Node();
        if (go == NodeToGameobjectMapping[currentNode])
        {
            return NodeType.CurrentlyStanding;
        }
        else if (GameobjectToNodeMapping.TryGetValue(go, out node))
        {
            if (DangeredNodes.Contains(node))
            {
                return NodeType.InDangerZone;
            }
            else if (!allowedNodesToMoveTo().Contains(node))
            {
                return NodeType.TooFar;
            }
            else
            {
                return NodeType.AllowedToMove;
            }
        }
        else
        {
            return NodeType.AllowedToMove;
        }
    }
}
