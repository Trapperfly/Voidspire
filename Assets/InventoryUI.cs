using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public Transform slotsParent;

    public Transform eqipmentSlotsParent;

    InventorySlot[] slots;

    EquipmentSlot[] equipmentSlots;

    public TMP_Text text;

    private void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = slotsParent.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = eqipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        List<int> loggedItems = new();
        for (int i = 0; i < slots.Length; i++) //Logging items
        {
            if (slots[i].item != null) { loggedItems.Add(slots[i].item.id); }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            //if (i < inventory.items.Count)
            //{
            //    slots[i].AddItem(inventory.items[i]);
            //} else
            //{
            //    slots[i].ClearSlot();
            //}

            if (slots[i].item != null) {  }
            else
            {
                for (int j = 0; j < inventory.items.Count; j++)
                {
                    if (loggedItems.Contains(inventory.items[j].id))
                    {
                        Debug.Log("It contained the thing");
                    }
                    else
                    {
                        slots[i].AddItem(inventory.items[j]);
                        loggedItems.Add(inventory.items[j].id);
                        Debug.Log("Added item " + inventory.items[j].name + " to slot " + i + ". It has id " + inventory.items[j].id);
                    }
                }
            }
            slots[i].Refresh();
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].Refresh();
        }
        text.text = new string(inventory.items.Count + " / " + slots.Length);
    }
}
