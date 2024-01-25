using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
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
                    newEquipment = RandomizeEquipment.Instance.RandomizeGun() as Weapon;
                    accepted = true;
                    break;
                case EquipmentTypes.Shield:
                    break;
                case EquipmentTypes.STL:
                    newEquipment = RandomizeEquipment.Instance.RandomizeSTLEngine() as STLEngine;
                    accepted = true;
                    break;
                case EquipmentTypes.FTL:
                    break;
                case EquipmentTypes.Hull:
                    break;
                case EquipmentTypes.Scanner:
                    break;
                case EquipmentTypes.Cargo:
                    break;
                case EquipmentTypes.Collector:
                    newEquipment = RandomizeEquipment.Instance.RandomizeCollector() as Collector;
                    accepted = true;
                    break;
                case EquipmentTypes.Relic:
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
