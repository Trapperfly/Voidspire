using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRelic : Events
{
    EquipmentController equipment;
    InventoryUI inventoryUI;
    Inventory inventory;
    public Equipment[] relics;

    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;
        equipment = EquipmentController.Instance;
        relics = new Equipment[equipment.relicSlots.Count];
        for (int i = 0; i < equipment.relicSlots.Count; i++)
        {
            relics[i] = ScriptableObject.CreateInstance("Equipment") as Equipment;
        }
        SetNewStats();

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }

    private void SetNewStats()
    {
        int i = 0;
        equipment.relicSlots.ForEach(slot =>
        {
            Equipment relic = null;
            if (slot.item != null) relic = relics[i];
            if (slot.item == null) { relics[i] = null; }
            else if (relics[i] == slot.item as Equipment) {  }
            else
            {
                if (relic != null) UnEquipRelic(relic);
                switch ((slot.item as Equipment).relic)
                {
                    case Relics.NotARelic:
                        Debug.Log("Either not a relic or not equipped");
                        relics[i] = null;
                        break;
                    case Relics.QuantumTargeting:
                        Debug.Log("QuantumTargeting is equipped");
                        relics[i] = slot.item as Equipment;
                        for (int i = 0; i < equipment.weaponSlots.Count; i++)
                        {
                            Weapon w = equipment.weaponSlots[i].item as Weapon;
                            w.homing = QuantumTargeting.setHomingToThis;
                            w.homingStrength += QuantumTargeting.aHomingStrength;
                            w.speed *= QuantumTargeting.mProjectileSpeed;
                            inventoryUI.slots[i].item = w;
                        }
                        for (int i = 0; i < inventoryUI.slots.Length; i++)
                        {
                            Debug.Log(inventoryUI.slots[i].item);
                            if (inventoryUI.slots[i].item != null && inventoryUI.slots[i].item is Weapon)
                            {
                                Weapon w = inventoryUI.slots[i].item as Weapon;
                                w.homing = QuantumTargeting.setHomingToThis;
                                w.homingStrength += QuantumTargeting.aHomingStrength;
                                w.speed *= QuantumTargeting.mProjectileSpeed;
                                inventoryUI.slots[i].item = w;
                            }
                        }
                        break;
                    case Relics.FriendModule:
                        Debug.Log("Friend Module is equipped");
                        relics[i] = slot.item as Equipment;
                        break;
                    case Relics.FissionBarrel:
                        Debug.Log("Fission Barrel is equipped");
                        relics[i] = slot.item as Equipment;
                        break;
                    case Relics.Default:
                        break;
                    default:
                        break;
                }
            }
            
            Debug.Log(i);
            i++;
        });
        
    }

    void UnEquipRelic(Equipment relic)
    {
        switch (relic.relic)
        {
            case Relics.NotARelic:
                break;
            case Relics.QuantumTargeting:
                for (int i = 0; i < equipment.weaponSlots.Count; i++)
                {
                    Weapon w = equipment.weaponSlots[i].item as Weapon;
                    w.homing = w.unalteredVersion.homing;
                    w.homingStrength -= QuantumTargeting.aHomingStrength;
                    w.speed *= 1/QuantumTargeting.mProjectileSpeed;
                    inventoryUI.slots[i].item = w;
                }
                for (int i = 0; i < inventoryUI.slots.Length; i++)
                {
                    Debug.Log(inventoryUI.slots[i].item);
                    if (inventoryUI.slots[i].item != null && inventoryUI.slots[i].item is Weapon)
                    {
                        Weapon w = inventoryUI.slots[i].item as Weapon;
                        w.homing = w.unalteredVersion.homing;
                        w.homingStrength -= QuantumTargeting.aHomingStrength;
                        w.speed *= 1 / QuantumTargeting.mProjectileSpeed;
                        inventoryUI.slots[i].item = w;
                    }
                }
                break;
            case Relics.FriendModule:
                break;
            case Relics.FissionBarrel:
                break;
            case Relics.Default:
                break;
            default:
                break;
        }
    }
}
