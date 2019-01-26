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

    void Start()
    {
        var mapTexture = GameObject.Find("MapArea").GetComponent<SpriteRenderer>().sprite.texture;
        var homeAreaTexture = GameObject.Find("HomePanel").GetComponent<SpriteRenderer>().size.x;
        mapWidth = mapTexture.width;
        mapHeight = mapTexture.height;

        // mapWidth = screenWidth - (homeAreaTexture * 32) as int;
        Debug.Log(homeAreaTexture);

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
                var offset = rand.Next(16, 32);
                offset = rand.Next(1) == 1 ? offset : -offset;
                var go = GameObject.Find("MapArea").transform;

                float xPosition = i * areaWidth + areaWidth / 2 + offset;

                offset = rand.Next(16, 32);
                offset = rand.Next(1) == 1 ? offset : -offset;
                float yPosition = mapHeight / nodesInThisArea.Count() / 2 * areaNodeCounter + offset;


                xPosition = xPosition - mapWidth/2 + homeAreaTexture;
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

        foreach (var kvp in NodeMapping)
        {
            var linerenderer = kvp.Value.GetComponent<LineRenderer>();
            var neighbourPositions = kvp.Key.Neighbours.Select(n => n.Position).ToArray();
            linerenderer.SetPositions(neighbourPositions);
            linerenderer.startColor = Color.white;
            linerenderer.endColor = Color.white;

            linerenderer.widthMultiplier = 0.07f;
            drawCircle(kvp.Value);
        }

        foreach (var node in Nodes)
        {
            //Debug.Log(string.Format("NodeId: {0}, NodeArea:{1}, Connecting to: {2}", node.id, node.Area, string.Join(", ", node.Neighbours.Select(n => string.Format("Id:{0} Area:{1}", n.id, n.Area)))));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int segments = 25;
    public int radius = 2;

    private void drawCircle(GameObject go)
    {
        int segments = 50;
        float radius = 0.5f;


        var s = new GameObject();
        s.transform.position = go.transform.position;
        var linerenderer = s.AddComponent<LineRenderer>();
        linerenderer.startColor = Color.white;
        linerenderer.endColor = Color.white;
        linerenderer.widthMultiplier = 0.07f;
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
}
