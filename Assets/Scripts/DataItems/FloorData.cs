using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData
{
    public FloorType Type;
    public int Health;
    public HomeUpgrade[] HomeUpgrades;
    public Dictionary<DamageTypes, int> Resistances;
    public Dictionary<DamageTypes, int> DamageBonuses;
    public int Comfort;
    public int StenchRemovalBonus;
    public float GlobalReloadSpeedBonus;
    public float GlobalHitChanceBonus;
    public int GlobalRandomHitsBonus;
    public List<int> GlobalRandomHits;

    public FloorData(int health)
    {
        HomeUpgrades = new HomeUpgrade[4];
        Health = health;
        Resistances = new Dictionary<DamageTypes, int>();
        Type = Utilities.UtilityFunctions.GetRandomElement(ContentManager.Instance.FloorTypes);
        UpdateStats();
    }

    public void UpdateStats()
    {
        Comfort = 0;
        StenchRemovalBonus = 0;
        GlobalReloadSpeedBonus = 0;
        GlobalHitChanceBonus = 0;
        GlobalRandomHitsBonus = 0;
        GlobalRandomHits = new List<int>();

        Resistances.Clear();
        for (int i = 0; i < 4; i++)
        {

            // Loop through every upgrade in the floor
            HomeUpgrade upgrade = HomeUpgrades[i];
            if (upgrade != null)
            {
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