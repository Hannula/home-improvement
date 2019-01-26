using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUpgrade
{
    public UpgradeData UpgradeData;
    public ModifierData ModifierData;
    public BonusModule Bonuses;

    public HomeUpgrade(UpgradeData upgrade)
    {
        UpgradeData = upgrade;
        UpdateBonuses();
    }

    public void UpdateBonuses()
    {
        if (ModifierData != null)
        {
            Bonuses = UpgradeData.BonusModule.Combine(ModifierData.BonusModule);
        }
        else
        {
            Bonuses = UpgradeData.BonusModule;
        }

        Bonuses.CompileDicts();
    }

    public string GetName()
    {
        string name;
        if (ModifierData != null)
        {
             name = ModifierData.Name + " " + UpgradeData.Name;
        }
        else
        {
            name = UpgradeData.Name;
        }
        return name;
    }

    public string GetDescription()
    {
        string desc = "";
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Physical))
        {
            desc += "<color=#ffffffff>" + Bonuses.ResistanceDict[DamageTypes.Physical] + " Physical Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Water))
        {
            desc += "<color=#add8e6ff>" + Bonuses.ResistanceDict[DamageTypes.Water] + " Water Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Fire))
        {
            desc += "<color=#ffa500ff>" + Bonuses.ResistanceDict[DamageTypes.Fire] + " Fire Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Noise))
        {
            desc += "<color=#ffff00ff>" + Bonuses.ResistanceDict[DamageTypes.Noise] + " Noise Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Stench))
        {
            desc += "<color=#808000ff>" + Bonuses.ResistanceDict[DamageTypes.Stench] + " Stench Resist</color>, ";
        }

        // DAMAGES

        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Physical))
        {
            desc += "<color=#ffffffff>" + Bonuses.DamageDict[DamageTypes.Physical] + " Physical Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Water))
        {
            desc += "<color=#add8e6ff>" + Bonuses.DamageDict[DamageTypes.Water] + " Water Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Fire))
        {
            desc += "<color=#ffa500ff>" + Bonuses.DamageDict[DamageTypes.Fire] + " Fire Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Noise))
        {
            desc += "<color=#ffff00ff>" + Bonuses.DamageDict[DamageTypes.Noise] + " Noise Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Stench))
        {
            desc += "<color=#808000ff>" + Bonuses.DamageDict[DamageTypes.Stench] + " Stench Damage</color>, ";
        }


        if (Bonuses.ComfortBonus != 0)
        {
            char sign = Bonuses.ComfortBonus > 0 ? ('+') : ('-');
            desc = desc + "<color=#00ff00ff>" + sign + Bonuses.ComfortBonus + " Comfort</color> ";
        }

        return desc;
    }
}
