using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public bool PlayerHome;
    public GameObject FloorPrefab;
    public List<Floor> Floors;
    public SpriteRenderer RoofSprite;

    public HomeData homeData;

    public void Awake()
    {
        Debug.Log("HOME INIT");
        if (PlayerHome)
        {
            homeData = GameManager.Instance.PlayerHome;
            homeData.Prune();
        }
        else
        {
            homeData = GameManager.Instance.EnemyHome;
        }
    }

    public void Init(HomeData homeData)
    {
        this.homeData = homeData;
    }

    public void Start()
    {
        if (homeData != null)
        {
            Floor prevFloor = null;
            for (int i = 0; i < homeData.Floors.Count; i++)
            {
                FloorData fd = homeData.Floors[i];
                Floor floor = GameObject.Instantiate(FloorPrefab, transform).GetComponent<Floor>();
                floor.FloorData = fd;
                floor.Index = i;
                floor.FloorLower = prevFloor;
                floor.MyHome = this;
                Floors.Add(floor);

                if (prevFloor != null)
                {
                    prevFloor.FloorUpper = floor;
                }

                prevFloor = floor;

            }
            prevFloor.RoofSprite = RoofSprite;
        }
    }

}
