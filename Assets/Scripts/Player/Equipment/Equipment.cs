using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public int level;
    public EquipmentTypes equipType;
    public Relics relic;

    public Quality quality;

    [TextArea]
    public string statsText;
    [TextArea]
    public string statsValues;

    public int statLength;

    public ParticleSystem.MinMaxGradient gradient;

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
    Thruster,
    //FTL,
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

public enum Relics
{
    NotARelic,
    QuantumTargeting,
    FriendModule,
    FissionBarrel,
    Default
}