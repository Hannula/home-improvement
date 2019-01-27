using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierData
{
    public string Name;
    public Color BlendColor;
    public int Tier;
    public BonusModule BonusModule;

    public string GetRarityColor()
    {
        switch(Tier)
        {
            case 1: return "#cccccc";
            case 2: return "#7fff4c";
            case 3: return "#4c84ff";
            case 4: return "#ffd84c";
        }
        return "#ff0000";
    }
}

