using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public bool PlayerHome;
    public GameObject FloorPrefab;
    public List<Floor> Floors;

    public HomeData homeData;

    public void Awake()
    {
        if (PlayerHome)
        {
            homeData = GameManager.Instance.PlayerHome;
            Debug.Log("Player Home!");
        }
    }

    public void Start()
    {
        if (homeData != null)
        {
            Debug.Log("Home Data found!");
            foreach (FloorData fd in homeData.Floors)
            {
                Floor floor = GameObject.Instantiate(FloorPrefab, transform).GetComponent<Floor>();
                floor.Data = fd;
                Floors.Add(floor);
            }
        }
    }

}
