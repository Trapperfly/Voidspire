using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Hull", menuName = "Inventory/Equipment/Hull")]
public class Hull : Equipment
{
    public HullTypes hullType;
    public float hullHealth;
    public float hullDamageNegation;
    public float hullWeight;
    public float hullSpecialEffectChance;
}
public enum HullTypes
{
    NebularProtection,
    NucularProtection,
    IONICProtection,
    Stealth,
    HeavyClass,
    LightClass,
    Barrier,
    ImpactInduction,
    Reactive,
    Sleek,
    Default
}
