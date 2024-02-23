using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int level;
    public EquipmentTypes type = EquipmentTypes.None;
    public bool random;
    public Transform parent;
    public Equipment newEquipment;
    bool accepted = false;
    private void Start()
    {
        parent = transform.parent;

        while (!accepted)
        {
            if (random)
            {
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
                    accepted = true;
                    break;
                case EquipmentTypes.Default:
                    break;
                default:
                    break;
            }
        }
    }
    public bool Pickup()
    {
        Debug.Log("Picking up " + newEquipment.name);
        return Inventory.Instance.Add(newEquipment);
    }
}
