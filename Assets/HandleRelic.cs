using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRelic : GameTrigger
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
            if (relics[i] != null) { relic = relics[i]; }
            if (slot.item == null) { if (relic != null) UnEquipRelic(relic); relics[i] = null; }
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
                        for (int j = 0; j < equipment.weaponSlots.Count; j++)
                        {
                            Weapon w = equipment.weaponSlots[j].item as Weapon;
                            w.homing = QuantumTargeting.setHomingToThis;
                            w.homingStrength += QuantumTargeting.aHomingStrength;
                            w.speed *= QuantumTargeting.mProjectileSpeed;
                            equipment.weaponSlots[j].item = w;
                        }
                        for (int j = 0; j < inventoryUI.slots.Length; j++)
                        {
                            Debug.Log(inventoryUI.slots[j].item);
                            if (inventoryUI.slots[j].item != null && inventoryUI.slots[j].item is Weapon)
                            {
                                Weapon w = inventoryUI.slots[j].item as Weapon;
                                w.homing = QuantumTargeting.setHomingToThis;
                                w.homingStrength += QuantumTargeting.aHomingStrength;
                                w.speed *= QuantumTargeting.mProjectileSpeed;
                                inventoryUI.slots[j].item = w;
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
                        for (int j = 0; j < equipment.weaponSlots.Count; j++)
                        {
                            Weapon w = equipment.weaponSlots[j].item as Weapon;
                            w.amount *= Mathf.RoundToInt(FissionBarrel.mProjectileCount);
                            w.spread *= FissionBarrel.mSpread;
                            w.rotationSpeed *= FissionBarrel.mRotSpeed;
                            equipment.weaponSlots[j].item = w;
                        }
                        for (int j = 0; j < inventoryUI.slots.Length; j++)
                        {
                            Debug.Log(inventoryUI.slots[j].item);
                            if (inventoryUI.slots[j].item != null && inventoryUI.slots[j].item is Weapon)
                            {
                                Weapon w = inventoryUI.slots[j].item as Weapon;
                                w.amount *= Mathf.RoundToInt(FissionBarrel.mProjectileCount);
                                w.spread *= FissionBarrel.mSpread;
                                w.rotationSpeed *= FissionBarrel.mRotSpeed;
                                inventoryUI.slots[j].item = w;
                            }
                        }
                        break;
                    case Relics.Default:
                        break;
                    default:
                        break;
                }
            }
            i++;
        });
        
    }

    void UnEquipRelic(Equipment relic)
    {
        Debug.Log("Unequipping " + relic.itemName);
        switch (relic.relic)
        {
            case Relics.NotARelic:
                break;
            case Relics.QuantumTargeting:
                for (int i = 0; i < equipment.weaponSlots.Count; i++)
                {
                    Weapon w = equipment.weaponSlots[i].item as Weapon;
                    if (CheckIfRelicIsEquipped(relic)) { }
                    else w.homing = w.savedHoming;
                    w.homingStrength -= QuantumTargeting.aHomingStrength;
                    w.speed *= 1/QuantumTargeting.mProjectileSpeed;
                    equipment.weaponSlots[i].item = w;
                }
                for (int i = 0; i < inventoryUI.slots.Length; i++)
                {
                    Debug.Log(inventoryUI.slots[i].item);
                    if (inventoryUI.slots[i].item != null && inventoryUI.slots[i].item is Weapon)
                    {
                        Weapon w = inventoryUI.slots[i].item as Weapon;
                        if (CheckIfRelicIsEquipped(relic)) { }
                        else w.homing = w.savedHoming;
                        w.homingStrength -= QuantumTargeting.aHomingStrength;
                        w.speed *= 1 / QuantumTargeting.mProjectileSpeed;
                        inventoryUI.slots[i].item = w;
                    }
                }
                break;
            case Relics.FriendModule:
                break;
            case Relics.FissionBarrel:
                for (int j = 0; j < equipment.weaponSlots.Count; j++)
                {
                    Weapon w = equipment.weaponSlots[j].item as Weapon;
                    w.amount /= Mathf.RoundToInt(FissionBarrel.mProjectileCount);
                    w.spread *= 1/FissionBarrel.mSpread;
                    w.rotationSpeed *= 1/FissionBarrel.mRotSpeed;
                    equipment.weaponSlots[j].item = w;
                }
                for (int j = 0; j < inventoryUI.slots.Length; j++)
                {
                    Debug.Log(inventoryUI.slots[j].item);
                    if (inventoryUI.slots[j].item != null && inventoryUI.slots[j].item is Weapon)
                    {
                        Weapon w = inventoryUI.slots[j].item as Weapon;
                        w.amount /= Mathf.RoundToInt(FissionBarrel.mProjectileCount);
                        w.spread *= 1/FissionBarrel.mSpread;
                        w.rotationSpeed *= 1/FissionBarrel.mRotSpeed;
                        inventoryUI.slots[j].item = w;
                    }
                }
                break;
            case Relics.Default:
                break;
            default:
                break;
        }
    }

    private bool CheckIfRelicIsEquipped(Equipment relic)
    {
        Debug.Log(relic);
        bool isSameRelic = false;
        for (int i = 0; i < relics.Length; i++)
            if (relics[i])
                isSameRelic = relics[i].relic == relic.relic;
        return isSameRelic;
    }
}
