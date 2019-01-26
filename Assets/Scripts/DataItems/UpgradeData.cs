using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData { 
    public Sprite Image;
    public string Name;
    public int Tier;
    public bool IsWeapon;
    public TargetingModule TargetingModule;
    public BonusModule BonusModule;

    public List<StatBonus> BaseDamage;

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
[System.Serializable]
public struct TargetingModule
{
    public float ReloadTime;
    public TargetTypes TargetingType;
    public List<int> Targets;
    public List<int> RandomTargets;
    public int RandomHits;
    public bool CanMiss;
    [Tooltip("From 0 to 100.")]
    public float HitChance;
}

[System.Serializable]
public struct BonusModule
{
    public int ComfortBonus;
    public int StenchRemovalBonus;
    public List<StatBonus> ResistanceBonuses;
    public List<StatBonus> GlobalDamageBonuses;
    public float GlobalReloadSpeedBonus;
    public float GlobalHitChanceBonus;
    public int GlobalRandomHitsBonus;
    public List<int> GlobalRandomHits;

    public Dictionary<DamageTypes, int> ResistanceDict;
    public Dictionary<DamageTypes, int> DamageDict;

    public void CompileDicts()
    {
        // Collect resistance bonuses
        ResistanceDict = new Dictionary<DamageTypes, int>();
        if (ResistanceBonuses != null)
        {
            foreach (StatBonus statBonus in ResistanceBonuses)
            {
                DamageTypes type = statBonus.DamageType;
                int amount = statBonus.Bonus;
                if (!ResistanceDict.ContainsKey(type))
                {
                    ResistanceDict[type] = amount;
                }
                else
                {
                    ResistanceDict[type] += amount;
                }
            }
        }
        DamageDict = new Dictionary<DamageTypes, int>();
        // Collect damage bonuses
        if (GlobalDamageBonuses != null)
        {
            foreach (StatBonus statBonus in GlobalDamageBonuses)
            {
                DamageTypes type = statBonus.DamageType;
                int amount = statBonus.Bonus;
                if (!DamageDict.ContainsKey(type))
                {
                    DamageDict[type] = amount;
                }
                else
                {
                    DamageDict[type] += amount;
                }
            }
        }
    }

    public BonusModule Combine(BonusModule otherModule)
    {
        BonusModule newModule = new BonusModule();
        newModule.ComfortBonus = ComfortBonus + otherModule.ComfortBonus;
        newModule.StenchRemovalBonus += StenchRemovalBonus + otherModule.StenchRemovalBonus;
        newModule.GlobalReloadSpeedBonus += GlobalReloadSpeedBonus + otherModule.GlobalReloadSpeedBonus;
        newModule.GlobalHitChanceBonus += GlobalHitChanceBonus + otherModule.GlobalHitChanceBonus;
        newModule.GlobalRandomHitsBonus += GlobalRandomHitsBonus + otherModule.GlobalRandomHitsBonus;
        newModule.GlobalRandomHits = new List<int>();
        newModule.GlobalRandomHits.AddRange(GlobalRandomHits);
        newModule.GlobalRandomHits.AddRange(otherModule.GlobalRandomHits);
        newModule.ResistanceBonuses = new List<StatBonus>();
        newModule.ResistanceBonuses.AddRange(ResistanceBonuses);
        newModule.ResistanceBonuses.AddRange(otherModule.ResistanceBonuses);

        newModule.GlobalDamageBonuses = new List<StatBonus>();
        newModule.GlobalDamageBonuses.AddRange(GlobalDamageBonuses);
        newModule.GlobalDamageBonuses.AddRange(otherModule.GlobalDamageBonuses);

        return newModule;
    }
}

[System.Serializable]
public struct StatBonus
{
    public DamageTypes DamageType;
    public int Bonus;
}

public enum TargetTypes
{
    None,
    Same,
    Top,
    Bottom
}