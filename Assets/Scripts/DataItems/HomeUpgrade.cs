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

    public HomeUpgrade(UpgradeData upgrade, ModifierData mod)
    {
        UpgradeData = upgrade;
        ModifierData = mod;
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

    public Color GetColor()
    {
        if (ModifierData == null)
        {
            return Color.white;
        }
        else
        {
            return ModifierData.BlendColor;
        }
    }

    public string GetDescription()
    {
        string desc = "";
        if (UpgradeData.IsWeapon)
        {
            desc += "<color=#ffffff>" + UpgradeData.TargetingModule.ReloadTime + "s Reload Time</color>, ";
            switch (UpgradeData.TargetingModule.TargetingType)
            {
                case TargetTypes.Same: {
                        desc += "<color=#14ff14> Target current floor, </color>, ";
                        break;
                    }
                case TargetTypes.Bottom:
                    {
                        desc += "<color=#14ff14> Target bottom floor, </color>, ";
                        break;
                    }
                case TargetTypes.Top:
                    {
                        desc += "<color=#14ff14> Target top floor, </color>, ";
                        break;
                    }
            }
            string floorsText = "<color=#214aff>Shoots at floors</color> <color=#ffffff>";
            foreach(int i in UpgradeData.TargetingModule.Targets)
            {
                floorsText += i + " ";
            }
            if (UpgradeData.TargetingModule.Targets.Count > 0)
            {
                desc += floorsText + "</color>, ";
            }

            string randomFloorsText = "<color=#214aff>Shoots randomly at floors</color> <color=#ffe36b>";
            foreach (int i in UpgradeData.TargetingModule.RandomTargets)
            {
                randomFloorsText += i + " ";
            }
            if (UpgradeData.TargetingModule.RandomTargets.Count > 0)
            {
                desc += randomFloorsText + "</color>, ";
            }

            desc += "<color=#214aff>" + UpgradeData.TargetingModule.HitChance + " Hit %</color>, ";
            desc += "\n";
        }

            
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Physical) && Bonuses.ResistanceDict[DamageTypes.Physical] != 0)
        {
            desc += "<color=#ffffffff>" + Bonuses.ResistanceDict[DamageTypes.Physical] + " Physical Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Water) && Bonuses.ResistanceDict[DamageTypes.Water] != 0)
        {
            desc += "<color=#add8e6ff>" + Bonuses.ResistanceDict[DamageTypes.Water] + " Water Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Fire) && Bonuses.ResistanceDict[DamageTypes.Fire] != 0)
        {
            desc += "<color=#ffa500ff>" + Bonuses.ResistanceDict[DamageTypes.Fire] + " Fire Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Noise) && Bonuses.ResistanceDict[DamageTypes.Noise] != 0)
        {
            desc += "<color=#ffff00ff>" + Bonuses.ResistanceDict[DamageTypes.Noise] + " Noise Resist</color>, ";
        }
        if (Bonuses.ResistanceDict.ContainsKey(DamageTypes.Stench) && Bonuses.ResistanceDict[DamageTypes.Stench] != 0)
        {
            desc += "<color=#808000ff>" + Bonuses.ResistanceDict[DamageTypes.Stench] + " Stench Resist</color>, ";
        }
        desc += "\n";

        // DAMAGES

        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Physical) && Bonuses.DamageDict[DamageTypes.Physical] != 0)
        {
            desc += "<color=#ffffffff>" + Bonuses.DamageDict[DamageTypes.Physical] + " Physical Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Water) && Bonuses.DamageDict[DamageTypes.Water] != 0)
        {
            desc += "<color=#add8e6ff>" + Bonuses.DamageDict[DamageTypes.Water] + " Water Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Fire) && Bonuses.DamageDict[DamageTypes.Fire] != 0)
        {
            desc += "<color=#ffa500ff>" + Bonuses.DamageDict[DamageTypes.Fire] + " Fire Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Noise) && Bonuses.DamageDict[DamageTypes.Noise] != 0)
        {
            desc += "<color=#ffff00ff>" + Bonuses.DamageDict[DamageTypes.Noise] + " Noise Damage</color>, ";
        }
        if (Bonuses.DamageDict.ContainsKey(DamageTypes.Stench) && Bonuses.DamageDict[DamageTypes.Stench] != 0)
        {
            desc += "<color=#808000ff>" + Bonuses.DamageDict[DamageTypes.Stench] + " Stench Damage</color>, ";
        }
        desc += "\n";


        if (Bonuses.ComfortBonus != 0)
        {
            char sign = Bonuses.ComfortBonus > 0 ? ('+') : (' ');
            desc = desc + "<color=#00ff00ff>" + sign + Bonuses.ComfortBonus + " Comfort</color> ";
        }
        if (Bonuses.StenchRemovalBonus != 0)
        {
            char sign = Bonuses.StenchRemovalBonus > 0 ? ('+') : (' ');
            desc = desc + "<color=#22ff89>" + sign + Bonuses.StenchRemovalBonus + " Stench Removal</color> ";
        }
        if (Bonuses.GlobalReloadSpeedBonus != 0)
        {
            char sign = Bonuses.GlobalReloadSpeedBonus > 0 ? ('+') : (' ');
            desc = desc + "<color=#ff4b23>" + sign + Bonuses.GlobalReloadSpeedBonus + "s Cooldown Reduction</color> ";
        }
        if (Bonuses.GlobalHitChanceBonus != 0)
        {
            char sign = Bonuses.GlobalHitChanceBonus > 0 ? ('+') : (' ');
            desc = desc + "<color=#214aff>" + sign + Bonuses.GlobalHitChanceBonus + " Hit % Bonus</color> ";
        }
        if (Bonuses.GlobalRandomHits != null && Bonuses.GlobalRandomHits.Count > 0 )
        {
            desc = desc + "<color=#ff602b> +" + Bonuses.GlobalRandomHits.Count + " Random Targets</color> ";
        }
        if (Bonuses.GlobalRandomHitsBonus != 0)
        {
            char sign = Bonuses.GlobalRandomHitsBonus > 0 ? ('+') : (' ');
            desc = desc + "<color=#ff432a>" + sign + Bonuses.GlobalRandomHitsBonus + " Random Shots</color> ";
        }

        return desc;
    }
}
