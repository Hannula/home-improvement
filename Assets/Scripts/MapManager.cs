using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using data;

public class MapManager : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public MapGenerator mapGenerator = new MapGenerator();
    public List<data.Node> Nodes = new List<Node>();
    public List<GameObject> NodeGameObjects = new List<GameObject>();
    public List<data.Node> LootedNodes = new List<Node>();
    public List<data.Node> DangeredNodes = new List<Node>();

    public Dictionary<data.Node, GameObject> NodeMapping = new Dictionary<Node, GameObject>();
    public Dictionary<GameObject, data.Node> GoToNodeMapping = new Dictionary< GameObject, data.Node>();

    public Transform nodePrefab;

    private System.Random rand = new System.Random();
    public Material lineMaterial;

    public int screenWidth = 640;

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

                //Debug.Log($"x:{xPosition}, y:{yPosition}");

                var homeGo = Instantiate(nodePrefab, node.Position, Quaternion.identity);
                NodeGameObjects.Add(homeGo.gameObject);
                NodeMapping.Add(node, homeGo.gameObject);
                GoToNodeMapping.Add(homeGo.gameObject, node);
                homeGo.gameObject.GetComponent<SelectableNode>().setNode(node);
                homeGo.SetParent(homeHolder.transform);
                homeGo.name = "HomeNode";
            }
        }

        if(!loadedSaveData)
        {
            currentNode = mapGenerator.HomeNode;
        }
        /*
        foreach (var kvp in NodeMapping)
        {
            var neighbourPositions = kvp.Key.Neighbours.Select(n => n.Position).ToArray();
            foreach (var pos in neighbourPositions)
            {
                var s = new GameObject();
                s.transform.position = kvp.Value.transform.position;
                s.transform.SetParent(lineHolder.transform);
                var linerenderer = s.AddComponent<LineRenderer>();
                linerenderer.startColor = Color.white;
                linerenderer.endColor = Color.white;
                linerenderer.widthMultiplier = 0.04f;
                linerenderer.material = lineMaterial;
                linerenderer.SetPosition(0, kvp.Value.transform.position);
                linerenderer.SetPosition(1, pos);
                linerenderer.sortingLayerName = "Lines";
                
            }

            // Debug.Log(string.Format("Drawing line NodeId: {0}, NodeArea:{1}, Connecting to: {2}", kvp.Key.id, kvp.Key.Area, string.Join(", ", kvp.Key.Neighbours.Select(n => string.Format("Id:{0} Area:{1} Pos:{2}", n.id, n.Area, n.Position.x*32)))));
        }
        */
        foreach (var node in Nodes)
        {
            //Debug.Log(string.Format("NodeId: {0}, NodeArea:{1}, Connecting to: {2}", node.id, node.Area, string.Join(", ", node.Neighbours.Select(n => string.Format("Id:{0} Area:{1}", n.id, n.Area)))));

        }

        drawNodeCircles();

        var obj = Instantiate(HomeIconPrefab, currentNode.Position, Quaternion.identity);
        HomeIcon = obj.gameObject;
        homeTarget = obj.transform;

        if (loadedSaveData)
        {
            advance -= 1;
            AdvanceDangerZone();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ActiveGame
            && HomeMoving)
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

    private List<GameObject> drawCircle(GameObject go, int segments, float radius, float width, Color col)
    {
        var lineObjects = new List<GameObject>();
        var s = new GameObject();
        s.transform.position = go.transform.position;
        lineObjects.Add(s);
        var linerenderer = s.AddComponent<LineRenderer>();
        linerenderer.startColor = col;
        linerenderer.endColor = col;
        linerenderer.widthMultiplier = width;

        if (go == NodeMapping[currentNode])
        {
            radius = 2 * radius;
            linerenderer.widthMultiplier = 2 * linerenderer.widthMultiplier;
        }

        var node = new Node();
        if (GoToNodeMapping.TryGetValue(go, out node))
        {
            if (LootedNodes.Contains(node) | !allowedNodesToMoveTo().Contains(node))
            {
                radius = radius * 0.8f;
                linerenderer.widthMultiplier = width;
                linerenderer.startColor = Color.gray;
                linerenderer.endColor = Color.gray;
            }

            if (DangeredNodes.Contains(node))
            {
                radius = radius * 0.8f;
                linerenderer.widthMultiplier = width;
                linerenderer.startColor = new Color(0.5f, 0, 0.3f);
                linerenderer.endColor = new Color(0.5f, 0, 0.3f);
            }
        }
       
        linerenderer.material = lineMaterial;

        linerenderer.positionCount = segments + 1;
        linerenderer.useWorldSpace = false;
        linerenderer.sortingLayerName = "Lines";

        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            linerenderer.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }

        return lineObjects;
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

    private GameObject dangerZoneGameObject;
    private int advance = 0;
    private List<GameObject> dangerCircle = new List<GameObject>();
    public void AdvanceDangerZone()
    {
        // Debug.Log("Advancing!");
        advance = advance + 1;
        if (advance == 1 | loadedSaveData)
        {
            dangerZoneGameObject = new GameObject();
            dangerZoneGameObject.transform.position = new Vector3(-400 / 32, -20 /32, 0);
        }

        if (advance == 2)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[1]);
        }
        else if (advance == 4)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[2]);
        }
        else if (advance == 6)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[3]);
        }
        else if (advance == 8)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[4]);
        }
        else if (advance == 10)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[5]);
        }
        else if (advance == 12)
        {
            DangeredNodes.AddRange(mapGenerator.AreaToNodeMapping[6]);
        }

        foreach (var s in dangerCircle)
        {
            Destroy(s);
        }

        dangerCircle = drawCircle(dangerZoneGameObject, 40, 8f + 1.5f * advance, 0.6f, Color.yellow);
    }


    public void MoveToNode(Node node)
    {
        LootedNodes.Add(currentNode);
        currentNode = node;
        homeTarget = NodeMapping[node].transform;
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
        eventManager.StartEvent(NodeMapping.Where(kvp => kvp.Value == t.gameObject).Select(kvp => kvp.Key).First());
    }

    private List<GameObject> nodeLines = new List<GameObject>();
    private void drawNodeCircles()
    {
        foreach (var line in nodeLines)
        {
            Destroy(line);
        }

        var radius = 0.06f;

        foreach (var kvp in NodeMapping)
        {
            var neighbourPositions = kvp.Key.Neighbours.Select(n => n.Position).ToArray();
            foreach (var pos in neighbourPositions)
            {
                var s = new GameObject();
                s.transform.position = kvp.Value.transform.position;
                s.transform.SetParent(lineHolder.transform);
                var linerenderer = s.AddComponent<LineRenderer>();
                linerenderer.startColor = Color.white;
                linerenderer.endColor = Color.white;
                linerenderer.widthMultiplier = 0.08f;
                linerenderer.material = lineMaterial;

                var radiusStartPos = kvp.Value.transform.position - (kvp.Value.transform.position - pos).normalized * (1+radius/(Vector3.Distance(kvp.Value.transform.position, pos)))/2.3f;
                var radiusEndPos = pos - (pos - kvp.Value.transform.position).normalized * (1+radius / (Vector3.Distance(kvp.Value.transform.position, pos)))/2.3f;
                Debug.Log("Radius: " + radius);
                Debug.Log("start->end rad/dist: " + radius / (Vector3.Distance(kvp.Value.transform.position, pos)));
                Debug.Log("StartPos: " + radiusStartPos);
                Debug.Log("EndPos: " + radiusEndPos);
                //linerenderer.SetPosition(0, kvp.Value.transform.position);
                linerenderer.SetPosition(0, radiusStartPos);
                //linerenderer.SetPosition(1, pos);
                linerenderer.SetPosition(1, radiusEndPos);
                linerenderer.sortingLayerName = "Lines";
                nodeLines.Add(s);

                
            }

            // Debug.Log(string.Format("Drawing line NodeId: {0}, NodeArea:{1}, Connecting to: {2}", kvp.Key.id, kvp.Key.Area, string.Join(", ", kvp.Key.Neighbours.Select(n => string.Format("Id:{0} Area:{1} Pos:{2}", n.id, n.Area, n.Position.x*32)))));
        }

        foreach (var kvp in NodeMapping)
        {
            nodeLines.AddRange(drawCircle(kvp.Value, 25, 0.25f, 0.12f, Color.red));
            // Debug.Log(string.Format("Drawing line NodeId: {0}, NodeArea:{1}, Connecting to: {2}", kvp.Key.id, kvp.Key.Area, string.Join(", ", kvp.Key.Neighbours.Select(n => string.Format("Id:{0} Area:{1} Pos:{2}", n.id, n.Area, n.Position.x*32)))));
        }
    }

}
