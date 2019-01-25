﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData
{
    public Sprite BackgroundImage;
    public int Health;
    public Dictionary<DamageTypes, int> Resistances;

    public FloorData(int health)
    {
        Health = health;
        Resistances = new Dictionary<DamageTypes, int>();
    }

    public void SetResistance(DamageTypes damageType, int resistance)
    {
        Resistances[damageType] = resistance;
    }

    public int GetResistance(DamageTypes damageType)
    {
        if (Resistances.ContainsKey(damageType))
        {
            return Resistances[damageType];
        }
        return 0;
    }
}

public enum DamageTypes
{
    Physical,
    Fire,
    Noise,
    Water,
    Stench,
}