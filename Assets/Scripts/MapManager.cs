using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int mapWidth;
    public MapGenerator mapGenerator = new MapGenerator();
    public List<Node> Nodes = new List<Node>();

    public Transform nodePrefab;

    void Start()
    {
        mapWidth = GameObject.Find("MapArea").GetComponent<SpriteRenderer>().sprite.texture.width;

        mapGenerator.Generate();
        Nodes = mapGenerator.AllNodes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
