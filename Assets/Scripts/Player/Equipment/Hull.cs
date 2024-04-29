using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Hull", menuName = "Inventory/Equipment/Hull")]
public class Hull : Equipment
{
    public HullTypes hullType;
    public int hullNodesMax;
    public int hullNodesCurrent;
    public float hullCurrentHealth;
    public float hullHealth;
    public float hullDamageNegation;
    public float hullWeight;
    public float hullSpecialEffectChance;
}
public enum HullTypes
{
    Default, //General
    HeavyClass, //High health, high weight
    LightClass, //Low health, low weight
    NebularProtection,
    NucularProtection,
    IONICProtection,
    Stealth,
    Barrier,
    ImpactInduction,
    Reactive,
    Sleek
}
