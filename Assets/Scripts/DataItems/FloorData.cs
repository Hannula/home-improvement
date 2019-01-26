using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData
{
    public FloorType Type;
    public int Health;
    public HomeUpgrade[] HomeUpgrades;
    public Dictionary<DamageTypes, int> Resistances;

    public FloorData(int health)
    {
        HomeUpgrades = new HomeUpgrade[4];
        Health = health;
        Resistances = new Dictionary<DamageTypes, int>();
        Type = Utilities.UtilityFunctions.GetRandomElement(ContentManager.Instance.FloorTypes);
        for (int i = 0; i < 4; i++)
        {
            if (Random.Range(0, 10) < 8)
            {
                HomeUpgrades[i] = new HomeUpgrade(Utilities.UtilityFunctions.GetRandomElement(ContentManager.Instance.Upgrades));
            }
        }
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