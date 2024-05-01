using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using static Inventory;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public delegate void OnInventoryLoad();
    public OnItemChanged onInventoryLoadCallback;

    Inventory inventory;

    public Transform inventoryGraphicsParent;

    public Transform slotsParent;

    public Transform eqipmentSlotsParent;

    public InventorySlot[] slots;

    public EquipmentSlot[] equipmentSlots;

    public TMP_Text text;

    public Transform slotsHud;
    public Image newItemIcon;

    #region Singleton
    public static InventoryUI Instance;

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
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = eqipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }
    #endregion
    private void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateUI;
        CheckStartingEquipment();
        UpdateUI();
    }

    public void CheckStartingEquipment()
    {
        foreach (var slot in equipmentSlots)
        {
            if (slot.item) inventory.equipment.Add(slot.item);
        }
    }

    private void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        List<int> loggedItems = new();
        for (int i = 0; i < slots.Length; i++) //Logging items
        {
            
            if (slots[i].item != null) { loggedItems.Add(slots[i].item.id);  }

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
            if (slots[i].item != null) {
            }
            else
            {
                for (int j = 0; j < inventory.items.Count; j++)
                {
                    if (!inventory.items[j]) { Debug.Log("Special case: deleted item i guess. If not deleted, then this is issue"); }
                    else if (loggedItems.Contains(inventory.items[j].id))
                    {
                        //Debug.Log("It contained the thing");
                    }
                    else
                    {
                        slots[i].AddItem(inventory.items[j]);
                        newItemIcon.color = new Color(1, 1, 1, 1);
                        loggedItems.Add(inventory.items[j].id);
                        //Debug.Log("Added item " + inventory.items[j].name + " to slot " + i + ". It has id " + inventory.items[j].id);
                    }
                }
            }
            slots[i].Refresh();
            for (int s = 0; s < slots.Length; s++)
            {
                Image slotHudIcon = slotsHud.GetChild(s).GetComponent<Image>();

                if (slots[s].item != null)
                {
                    slotHudIcon.color = new Color(1, 1, 1, 1);
                }
                else slotHudIcon.color = new Color(1, 1, 1, 0);
            }
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].Refresh();
        }
        text.text = inventory.items.Count.ToString() + "/" + inventory.inventorySpace.ToString();
    }
}
