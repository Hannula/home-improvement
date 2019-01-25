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
}
