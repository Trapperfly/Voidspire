using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentTypes equipType;

    public Quality quality;

    public override void Use()
    {
        base.Use();
    }
}
public enum EquipmentTypes
{
    None,
    All,
    Weapon,
    Shield,
    STL,
    FTL,
    Hull,
    Scanner,
    Cargo,
    Collector,
    Relic,
    Default
}

public enum Quality
{
    Scrap,
    Poor,
    Normal,
    Quality,
    Pristine,
}