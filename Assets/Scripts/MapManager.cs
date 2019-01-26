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

    public Transform nodePrefab;

    private System.Random rand = new System.Random();

    public int screenWidth = 640;

    void Start()
    {
        var mapTexture = GameObject.Find("MapArea").GetComponent<SpriteRenderer>().sprite.texture;
        var homeAreaTexture = GameObject.Find("HomeArea").GetComponent<SpriteRenderer>().sprite.texture;
        mapWidth = mapTexture.width;
        mapHeight = mapTexture.height;

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
                float yPosition = mapHeight / nodesInThisArea.Count() / 2 * areaNodeCounter + offset;


                xPosition = xPosition - mapWidth/2 + homeAreaTexture.width/2;
                yPosition = yPosition - mapHeight/2/2;

                // Pixel space to screen space
                xPosition = xPosition / 32;
                yPosition = yPosition / 32;

                //Debug.Log($"x:{xPosition}, y:{yPosition}");

                var obj = Instantiate(nodePrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);                
            }

            
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
}
