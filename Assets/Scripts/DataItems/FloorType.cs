using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorType 
{
    public string Name;
    public Sprite BackgroundImage;
    public int BaseHealth;
    public int BaseComfort;
    public BonusModule Bonuses;
    public int Tier;
}
