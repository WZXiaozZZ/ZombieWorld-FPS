using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum AttributeType
{ 
    speedLevel=0,
    hpLevel,
    atkLevel,
    rateOfFireLevel,
    replenishmentDropRateLevel,
    lifestealLevel,
    healthRegenRateLevel,
    defense,
    autoAmmoRegeneration,
}


public class Attribute 
{
    private int speedLevel;
    public int SpeedLevel { get  { return speedLevel; } set { speedLevel += value; } }

    private int hpLevel;
    public int HPLevel { get { return hpLevel; } set { hpLevel+= value; } }

    private int atkLevel;
    public int AtkLevel { get { return atkLevel; }set { atkLevel += value; } }
    private int rateOfFireLevel;
    public int RateOfFireLevel { get { return rateOfFireLevel; } set { rateOfFireLevel += value; } }

    private int replenishmentDropRateLevel;
    public int ReplenishmentDropRateLevel { get { return replenishmentDropRateLevel; } set { replenishmentDropRateLevel += value; } }
    private int lifestealLevel;
    public int LifestealLevel { get { return lifestealLevel; } set { lifestealLevel += value; } }
    private int healthRegenRateLevel;
    public int HealthRegenRateLevel { get { return healthRegenRateLevel; } set { healthRegenRateLevel += value; } }
    private int defense;
    public int Defense { get { return defense; } set { defense += value; } }
    private int autoAmmoRegenerationLevel;
    public int AutoAmmoRegenerationLevel { get { return autoAmmoRegenerationLevel; } }
    public Attribute(int speedLevel,int hpLevel,int atkLevel,int rateOfFireLevel,int replenishmentDropRateLevel,int lifestealLevel,int healthRegenRateLevel ,int defense,int autoAmmoRegenerationLevel) 
    {
        this.speedLevel = speedLevel;
        this.hpLevel = hpLevel;
        this.atkLevel = atkLevel;
        this.rateOfFireLevel = rateOfFireLevel;
        this.replenishmentDropRateLevel = replenishmentDropRateLevel;
        this.lifestealLevel = lifestealLevel;
        this.healthRegenRateLevel = healthRegenRateLevel;
        this.defense = defense;
        this.autoAmmoRegenerationLevel = autoAmmoRegenerationLevel;
    }

    public int GetLevel(int index) 
    {
        switch ((AttributeType)index)
        {
            case AttributeType.speedLevel:
                return speedLevel;
            case AttributeType.hpLevel:
                return hpLevel;
            case AttributeType.atkLevel:
                return atkLevel;
            case AttributeType.rateOfFireLevel:
                return rateOfFireLevel;
            case AttributeType.replenishmentDropRateLevel:
                return replenishmentDropRateLevel;
            case AttributeType.lifestealLevel:
                return lifestealLevel;
            case AttributeType.healthRegenRateLevel:
                return healthRegenRateLevel;
            case AttributeType.defense:
                return defense;
            case AttributeType.autoAmmoRegeneration:
                return autoAmmoRegenerationLevel;
            default:
                break;
        }
        return 0;
    }

    public void UpgradesSelect(int index) 
    {
        switch ((AttributeType)index)
        {
            case AttributeType.speedLevel:
                speedLevel++;
                Player.Instance.MovespeedLevelUpgrades();
                break;
            case AttributeType.hpLevel:
                hpLevel++;
                Player.Instance.AddMaxHP();
                break;
            case AttributeType.atkLevel:
                atkLevel++;
                break;
            case AttributeType.rateOfFireLevel:
                rateOfFireLevel++;
                break;
            case AttributeType.replenishmentDropRateLevel:
                replenishmentDropRateLevel++;
                break;
            case AttributeType.lifestealLevel:
                lifestealLevel++;
                break;
            case AttributeType.healthRegenRateLevel:
                healthRegenRateLevel++;
                break;
            case AttributeType.defense:
                defense++;
                break;
            case AttributeType.autoAmmoRegeneration:
                autoAmmoRegenerationLevel++;
                Player.Instance.SetAutoLevel(autoAmmoRegenerationLevel);
                break;
            default:
                break;
        }
    }
    public void UpgradesSelectPlane(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.speedLevel:
                speedLevel++;
                break;
            case AttributeType.hpLevel:
                hpLevel++;
                break;
            case AttributeType.atkLevel:
                atkLevel++;
                break;
            case AttributeType.rateOfFireLevel:
                rateOfFireLevel++;
                break;
            case AttributeType.replenishmentDropRateLevel:
                replenishmentDropRateLevel++;
                break;
            case AttributeType.lifestealLevel:
                lifestealLevel++;
                break;
            case AttributeType.healthRegenRateLevel:
                healthRegenRateLevel++;
                break;
            case AttributeType.defense:
                defense++;
                break;
            case AttributeType.autoAmmoRegeneration:
                autoAmmoRegenerationLevel++;
                break;
            default:
                break;
        }
    }
}
