using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int level;
    public EquipmentTypes type = EquipmentTypes.None;
    public bool random;
    public float additionalWeaponChance;
    public float relicChance;
    public Transform parent;
    public Equipment newEquipment;
    bool accepted = false;

    public ParticleSystem ps;
    public SpriteRenderer sprite;
    private void Start()
    {
        parent = transform.parent;

        while (!accepted)
        {
            if (random)
            {
                if (Random.value < additionalWeaponChance)
                {
                    type = EquipmentTypes.Weapon;
                    newEquipment = RandomizeEquipment.Instance.RandomizeGun(level) as Weapon;
                    ChangeTrailAndSpriteColors();
                    return;
                }
                type = (EquipmentTypes)Random.Range(2, 10);
            }
            switch (type)
            {
                case EquipmentTypes.None:
                    break;
                case EquipmentTypes.All:
                    break;
                case EquipmentTypes.Weapon:
                    newEquipment = RandomizeEquipment.Instance.RandomizeGun(level) as Weapon;
                    accepted = true;
                    break;
                case EquipmentTypes.Shield:
                    newEquipment = RandomizeEquipment.Instance.RandomizeShield(level) as Shield;
                    accepted = true;
                    break;
                case EquipmentTypes.Thruster:
                    newEquipment = RandomizeEquipment.Instance.RandomizeThruster(level) as Thrusters;
                    accepted = true;
                    break;
                //case EquipmentTypes.FTL:
                //    newEquipment = RandomizeEquipment.Instance.RandomizeFTLEngine() as FTLEngine;
                //    accepted = true;
                //    break;
                case EquipmentTypes.Hull:
                    newEquipment = RandomizeEquipment.Instance.RandomizeHull(level) as Hull;
                    accepted = true;
                    break;
                case EquipmentTypes.Scanner:
                    newEquipment = RandomizeEquipment.Instance.RandomizeScanner(level) as Scanner;
                    accepted = true;
                    break;
                case EquipmentTypes.Cargo:
                    break;
                case EquipmentTypes.Collector:
                    newEquipment = RandomizeEquipment.Instance.RandomizeCollector(level) as Collector;
                    accepted = true;
                    break;
                case EquipmentTypes.Relic:
                    newEquipment = RandomizeEquipment.Instance.RandomizeRelic();
                    if (Random.value > relicChance && random)
                        accepted = false;
                    else accepted = true;
                    break;
                case EquipmentTypes.Default:
                    break;
                default:
                    break;
            }
        }
        ChangeTrailAndSpriteColors();
    }

    void ChangeTrailAndSpriteColors()
    {
        var trail = ps.trails;
        trail.colorOverLifetime = newEquipment.gradient;

        sprite.color = newEquipment.color;
    }
    public bool Pickup()
    {
        //Debug.Log("Picking up " + newEquipment.name);
        return Inventory.Instance.Add(newEquipment);
    }
}
