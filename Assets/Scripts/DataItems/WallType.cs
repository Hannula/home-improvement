using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallType
{
    public string Name;
    public Sprite BackgroundImage;
    public int HealthBonus;
    public int ComfortBonus;
    public BonusModule Bonuses;
    public int Tier;
}
