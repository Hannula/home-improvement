using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData
{
    public FloorType Type;
    public WallType WallType;
    public int Health;
    public HomeUpgrade[] HomeUpgrades;
    public Dictionary<DamageTypes, int> Resistances;
    public Dictionary<DamageTypes, int> DamageBonuses;
    public int Comfort;
    public int BaseComfort;
    public int StenchRemovalBonus;
    public float GlobalReloadSpeedBonus;
    public float GlobalHitChanceBonus;
    public int GlobalRandomHitsBonus;
    public List<int> GlobalRandomHits;
    public float MaxCooldown;
    public int StenchLevel;

    public FloorData(FloorType floorType, WallType wallType)
    {
        HomeUpgrades = new HomeUpgrade[4];
        Type = floorType;
        WallType = wallType;
        Health = floorType.BaseHealth + wallType.HealthBonus;
        Resistances = new Dictionary<DamageTypes, int>();
        DamageBonuses = new Dictionary<DamageTypes, int>();
        UpdateStats();
    }

    public void UpdateStats()
    {
        MaxCooldown = 0f;
        Comfort = Type.BaseComfort + WallType.ComfortBonus + Type.Bonuses.ComfortBonus + WallType.Bonuses.ComfortBonus;
        BaseComfort = Comfort;
        StenchRemovalBonus = Type.Bonuses.StenchRemovalBonus + WallType.Bonuses.StenchRemovalBonus;
        GlobalReloadSpeedBonus = Type.Bonuses.GlobalReloadSpeedBonus + WallType.Bonuses.GlobalReloadSpeedBonus;
        GlobalHitChanceBonus =  Type.Bonuses.GlobalHitChanceBonus + WallType.Bonuses.GlobalHitChanceBonus; ;
        GlobalRandomHitsBonus =  Type.Bonuses.GlobalRandomHitsBonus + WallType.Bonuses.GlobalRandomHitsBonus; ;
        GlobalRandomHits = new List<int>();

        Resistances.Clear();
        for (int i = 0; i < 4; i++)
        {

            // Loop through every upgrade in the floor
            HomeUpgrade upgrade = HomeUpgrades[i];
            if (upgrade != null)
            {
                float time = upgrade.UpgradeData.TargetingModule.ReloadTime;
                if (upgrade.UpgradeData.IsWeapon)
                {
                    MaxCooldown = Mathf.Max(time, MaxCooldown);
                }
                BonusModule bonusModule = upgrade.UpgradeData.BonusModule;
                // Comfort
                Comfort += bonusModule.ComfortBonus;
                StenchRemovalBonus += bonusModule.StenchRemovalBonus;
                GlobalReloadSpeedBonus += bonusModule.GlobalReloadSpeedBonus;
                GlobalHitChanceBonus += bonusModule.GlobalHitChanceBonus;
                GlobalRandomHitsBonus += bonusModule.GlobalRandomHitsBonus;
                GlobalRandomHits.AddRange(bonusModule.GlobalRandomHits);

                // Collect resistance bonuses
                foreach (StatBonus statBonus in upgrade.UpgradeData.BonusModule.ResistanceBonuses)
                {
                    DamageTypes type = statBonus.DamageType;
                    int amount = statBonus.Bonus;
                    if (!Resistances.ContainsKey(type))
                    {
                        Resistances[type] = amount;
                    }
                    else
                    {
                        Resistances[type] += amount;
                    }
                }

                // Collect damage bonuses
                foreach (StatBonus statBonus in upgrade.UpgradeData.BonusModule.GlobalDamageBonuses)
                {
                    DamageTypes type = statBonus.DamageType;
                    int amount = statBonus.Bonus;
                    if (!DamageBonuses.ContainsKey(type))
                    {
                        DamageBonuses[type] = amount;
                    }
                    else
                    {
                        DamageBonuses[type] += amount;
                    }
                }
            }
        }
        StenchLevel = 0;
        MaxCooldown -= GlobalReloadSpeedBonus;
    }

    public void ResetComfort()
    {
        StenchLevel = 0;
        Comfort = BaseComfort;
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

    public void DealDamage(List<StatBonus> dmg)
    {
        int healthDamage = 0;
        int comfortDamage = 0;
        foreach (StatBonus stat in dmg)
        {
            DamageTypes type = stat.DamageType;
            int statDamage = stat.Bonus;
            if (statDamage > 0)
            {
                if (type == DamageTypes.Fire || type == DamageTypes.Physical || type == DamageTypes.Water)
                {
                    healthDamage += Mathf.Max(0, statDamage - GetResistance(type));
                }
                else if (type == DamageTypes.Noise)
                {
                    comfortDamage += Mathf.Max(0, statDamage - GetResistance(type));
                }
                else
                {
                    StenchLevel += Mathf.Max(0, statDamage - GetResistance(type));
                }
            }
        }
        Health -= healthDamage;
        Comfort -= comfortDamage;
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