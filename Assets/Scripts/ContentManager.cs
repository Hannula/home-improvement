using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
public class ContentManager : MonoBehaviour
{
    public static ContentManager Instance;

    public List<UpgradeData> Upgrades;

    public List<ModifierData> Modifiers;

    public List<WallType> WallTypes;

    public List<FloorType> FloorTypes;

    public UpgradeData GetRandomUpgradeData(int tier)
    {
        List<UpgradeData> tierUpgrades = new List<UpgradeData>();
        while (tier > 0 && tierUpgrades.Count <= 0)
        {
            tierUpgrades = Upgrades.Where(x => x.Tier == tier).ToList();
            tier -= 1;
        }
        if (tierUpgrades.Count <= 0)
        {
            return null;
        }
        return UtilityFunctions.GetRandomElement(tierUpgrades);
    }

    public ModifierData GetRandomModifierData(int tier)
    {
        List<ModifierData> tierUpgrades = new List<ModifierData>();
        while (tier > 0 && tierUpgrades.Count <= 0)
        {
            tierUpgrades = Modifiers.Where(x => x.Tier == tier).ToList();
            tier -= 1;
        }
        if (tierUpgrades.Count <= 0)
        {
            return null;
        }
        return UtilityFunctions.GetRandomElement(tierUpgrades);
    }

    public WallType GetRandomWallType(int tier)
    {
        List<WallType> tierUpgrades = new List<WallType>();
        while (tier > 0 && tierUpgrades.Count <= 0)
        {
            tierUpgrades = WallTypes.Where(x => x.Tier == tier).ToList();
            tier -= 1;
        }
        if (tierUpgrades.Count <= 0)
        {
            return null;
        }
        return UtilityFunctions.GetRandomElement(tierUpgrades);
    }

    public FloorType GetRandomFloorType(int tier)
    {
        List<FloorType> tierUpgrades = new List<FloorType>();
        while (tier > 0 && tierUpgrades.Count <= 0)
        {
            tierUpgrades = FloorTypes.Where(x => x.Tier == tier).ToList();
            tier -= 1;
        }
        if (tierUpgrades.Count <= 0)
        {
            return null;
        }
        return UtilityFunctions.GetRandomElement(tierUpgrades);
    }


    public HomeUpgrade GetRandomHomeUpgrade(int furnitureTier, int modifierTier)
    {
        return new HomeUpgrade(GetRandomUpgradeData(furnitureTier), GetRandomModifierData(modifierTier));
    }

    void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


}
