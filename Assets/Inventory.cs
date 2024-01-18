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

        if (slot0.item != null) { slot1.item = slot0.item; }
        else { slot1.item = null; }

        if (slot1.item != null) { slot0.item = itemTemp; }
        else { slot0.item = null; }

        onItemChangedCallback?.Invoke();
    }
}
