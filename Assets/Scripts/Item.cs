using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string itemName;
    public string description;
    public Sprite icon;
    public bool isDefault;
    public int id;

    public virtual void Use()
    {
        Debug.Log("UsingItem");
    }
}
