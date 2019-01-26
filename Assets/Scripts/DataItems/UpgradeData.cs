using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData { 
    public Sprite Image;
    public string Name;
    public int Tier;

    public Color GetColor()
    {
        Color col = Color.white;

        switch (Tier) {
            case 2: col = Color.green;
                break;
            case 3:
                col = Color.blue;
                break;
            case 4:
                col = Color.red;
                break;
        };

        return col;
    }
}
