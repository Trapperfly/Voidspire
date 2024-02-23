using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRelic : Events
{
    EquipmentController equipment;

    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        equipment = EquipmentController.Instance;
        SetNewStats();

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }

    private void SetNewStats()
    {
        int i = 0;
        equipment.relicSlots.ForEach(slot =>
        {
            if (slot.item == null) return;
            switch ((slot.item as Equipment).relic)
            {
                case Relics.NotARelic:
                    Debug.Log("Either not a relic or not equipped");
                    break;
                case Relics.QuantumTargeting:
                    Debug.Log("QuantumTargeting is equipped");
                    break;
                case Relics.FriendModule:
                    Debug.Log("Friend Module is equipped");
                    break;
                case Relics.FissionBarrel:
                    Debug.Log("Fission Barrel is equipped");
                    break;
                case Relics.Default:
                    break;
                default:
                    break;
            }
            i++;
        });
        
    }
}
