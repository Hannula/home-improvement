using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonData
{
    public string Name = "Arse";
    public float Comfort = 10f;
    public float Health = 10f;
    public float Skill = 10f;

    public PersonData(string name, float health, float comfort, float skill)
    {
        Name = name;
        Comfort = comfort;
        Health = health;
        Skill = skill;
    }
}
