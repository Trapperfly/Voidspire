using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public EquipmentTypes type = EquipmentTypes.None;
    public Transform parent;
    public Equipment newEquipment;
    private void Start()
    {
        parent = transform.parent;
        switch (type)
        {
            case EquipmentTypes.None:
                break;
            case EquipmentTypes.All:
                break;
            case EquipmentTypes.Weapon:
                newEquipment = RandomizeEquipment.Instance.RandomizeGun() as Weapon;
                break;
            case EquipmentTypes.Shield:
                break;
            case EquipmentTypes.STL:
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
                break;
            case EquipmentTypes.Relic:
                break;
            case EquipmentTypes.Default:
                break;
            default:
                break;
        }
    }
    public bool Pickup()
    {
        Debug.Log("Picking up " + newEquipment.name);
        return Inventory.Instance.Add(newEquipment);
    }
}
