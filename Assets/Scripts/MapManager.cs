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

    public Dictionary<data.Node, GameObject> NodeMapping = new Dictionary<Node, GameObject>();

    public Transform nodePrefab;

    private System.Random rand = new System.Random();
    public Material lineMaterial;

    public int screenWidth = 640;

    public Node currentNode;

    void Start()
    {
        var mapTexture = GameObject.Find("MapArea").GetComponent<SpriteRenderer>().sprite.texture;
        var homeAreaTexture = GameObject.Find("HomePanel").GetComponent<SpriteRenderer>().size.x;
        mapWidth = mapTexture.width;
        mapHeight = mapTexture.height;

        var homeAreaWidthPixels = (int)Math.Round((homeAreaTexture * 32));
        mapWidth = screenWidth - homeAreaWidthPixels;

        // Debug.Log(mapWidth);

        mapGenerator.Generate();
        Nodes = mapGenerator.AllNodes;

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

                float xPosition = i * areaWidth + areaWidth / 2 + offset;

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

                var obj = Instantiate(nodePrefab, node.Position, Quaternion.identity);
                NodeGameObjects.Add(obj.gameObject);
                NodeMapping.Add(node, obj.gameObject);
                obj.gameObject.GetComponent<SelectableNode>().setNode(node);
            }
        }

        currentNode = mapGenerator.HomeNode;


        foreach (var kvp in NodeMapping)
        {
            var neighbourPositions = kvp.Key.Neighbours.Select(n => n.Position).ToArray();
            foreach (var pos in neighbourPositions)
            {
                var s = new GameObject();
                s.transform.position = kvp.Value.transform.position;
                lineObjects.Add(s);
                var linerenderer = s.AddComponent<LineRenderer>();
                linerenderer.startColor = Color.white;
                linerenderer.endColor = Color.white;
                linerenderer.widthMultiplier = 0.04f;
                linerenderer.material = lineMaterial;
                linerenderer.SetPosition(0, kvp.Value.transform.position);
                linerenderer.SetPosition(1, pos);
                // Debug.Log(pos);
            }

            drawCircle(kvp.Value, 25, 0.25f);
            // Debug.Log(string.Format("Drawing line NodeId: {0}, NodeArea:{1}, Connecting to: {2}", kvp.Key.id, kvp.Key.Area, string.Join(", ", kvp.Key.Neighbours.Select(n => string.Format("Id:{0} Area:{1} Pos:{2}", n.id, n.Area, n.Position.x*32)))));
        }

        foreach (var node in Nodes)
        {
            //Debug.Log(string.Format("NodeId: {0}, NodeArea:{1}, Connecting to: {2}", node.id, node.Area, string.Join(", ", node.Neighbours.Select(n => string.Format("Id:{0} Area:{1}", n.id, n.Area)))));

        }

    }


    private float secondsPassed = 0;
    // Update is called once per frame
    void Update()
    {
        secondsPassed += GameManager.Instance.DeltaTime;
        if (secondsPassed > 5)
        {
            secondsPassed = 0;
            AdvanceDangerZone();
        }
    }

    private List<GameObject> lineObjects = new List<GameObject>();

    private void drawCircle(GameObject go, int segments, float radius)
    {
        var s = new GameObject();
        s.transform.position = go.transform.position;
        lineObjects.Add(s);
        var linerenderer = s.AddComponent<LineRenderer>();
        linerenderer.startColor = Color.red;
        linerenderer.endColor = Color.red;
        linerenderer.widthMultiplier = 0.04f;

        if (go == NodeMapping[currentNode])
        {
            radius = 2 * radius;
            linerenderer.widthMultiplier = 2* linerenderer.widthMultiplier;
        }

        linerenderer.material = lineMaterial;

        linerenderer.positionCount = segments + 1;
        linerenderer.useWorldSpace = false;

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
    }

    public List<GameObject> allowedNodeToMoveTo()
    {
        var list = new List< GameObject > ();
        foreach (var n in currentNode.Neighbours)
        {
            list.Add(NodeMapping[n]);
        }

        return list;
    }

    private GameObject dangerZoneGameObject;
    private int advance = 0;
    public void AdvanceDangerZone()
    {
        Debug.Log("Advancing!");
        advance = advance + 1;
        if (advance == 1)
        {
            dangerZoneGameObject = new GameObject();
            dangerZoneGameObject.transform.position = new Vector3(-400 / 32, -20 /32, 0);
        }
         
        drawCircle(dangerZoneGameObject, 25, 8.5f + advance);
    }
}
