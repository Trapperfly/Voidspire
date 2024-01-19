using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Transform parent;
    public Weapon newWeapon;
    private void Start()
    {
        parent = transform.parent;
        newWeapon = RandomizeEquipment.Instance.RandomizeGun();
    }
    public bool Pickup()
    {
        Debug.Log("Picking up " + newWeapon.name);
        return Inventory.Instance.Add(newWeapon);
    }
}
