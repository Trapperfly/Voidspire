using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Item item;
    public Transform parent;
    private void Awake()
    {
        parent = transform.parent;
    }
    public bool Pickup()
    {
        Debug.Log("Picking up " +  item.name);
        return Inventory.Instance.Add(item);
    }
}
