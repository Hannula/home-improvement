using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeData
{
    public List<FloorData> Floors;
    public List<PersonData> Residents;

    public HomeData()
    {
        Floors = new List<FloorData>();
        Residents = new List<PersonData>();
    }

    public HomeData(params FloorData[] floors)
    {
        Floors = floors.ToList();
    }

    public static HomeData GenerateRandom(int maxTier, int floors, float roomChance = 50)
    {
        List<FloorData> floorDataList = new List<FloorData>();
        for(int i = 0; i < floors; i++)
        {
            FloorData floorData = new FloorData(
                ContentManager.Instance.GetRandomFloorType(Random.Range((int)1, (int)maxTier)),
                ContentManager.Instance.GetRandomWallType(Random.Range((int)1, (int)maxTier))
                );
            for (int j = 0; j < 4; j++)
            {
                if (Random.Range(0, 100) < roomChance)
                {
                    floorData.HomeUpgrades[j] = ContentManager.Instance.GetRandomHomeUpgrade(Random.Range((int)1, (int)maxTier), Random.Range((int)1, (int)maxTier));
                }

            }
            floorDataList.Add(floorData);
            floorData.UpdateStats();
        }
        HomeData home = new HomeData();
        home.Floors = floorDataList;
        return home;
    }

    public void Prune()
    {
        for(int i = Floors.Count - 1; i >= 0; i--)
        {
            FloorData data = Floors[i];
            if (data.Health <= 0)
            {
                Floors.RemoveAt(i);
            }
        }
    }
}
