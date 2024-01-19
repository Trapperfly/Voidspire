using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int inventorySpace;

    public int id = 1;

    public List<Item> items = new();
    public List<Item> equipment = new();

    public bool Add (Item item)
    {
        if (!item.isDefault) 
        {
            if (items.Count >= inventorySpace)
            {
                Debug.Log("Not enough inventory space");
                return false;
            }
            item.id = id;
            id++;
            Debug.Log(item.id);
            items.Add(item);

            onItemChangedCallback?.Invoke();
        }
        return true;
    }

    public void Remove (Item item)
    {
        items.Remove(item);
    }

    public void Swap(InventorySlot slot0, InventorySlot slot1)
    {
        Item itemTemp = slot1.item;

        bool s0Equip = false;
        bool s1Equip = false;

        bool s0Filled = false;
        bool s1Filled = false;

        if (slot0 is EquipmentSlot) { s0Equip = true; }
        if (slot1 is EquipmentSlot) { s1Equip = true; }

        if (slot0.item != null) { s0Filled = true; }
        if (slot1.item != null) { s1Filled = true; }

        if (s0Filled) 
        { 
            slot1.item = slot0.item;
            if (s1Equip && !s0Equip) { items.Remove(slot0.item); equipment.Add(slot0.item); }
            else if (!s1Equip && s0Equip) { equipment.Remove(slot0.item); items.Add(slot0.item); }
            
        }
        else { Debug.Log("Slot 0 was empty, so slot 1 is set to null"); slot1.item = null; }

        if (s1Filled) 
        { 
            slot0.item = itemTemp;
            if (s0Equip && !s1Equip) { items.Remove(itemTemp); equipment.Add(itemTemp); }
            else if (!s0Equip && s1Equip) { equipment.Remove(itemTemp); items.Add(itemTemp); }
        }
        else { Debug.Log("Slot 1 was empty, so slot 0 is set to null"); slot0.item = null; }

        onItemChangedCallback?.Invoke();
    }
}
