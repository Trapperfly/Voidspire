using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldToSalvage : MonoBehaviour
{
    public InventorySlot slot;
    public bool salvagable;
    bool isEquipmentSlot;
    public float salvageSpeed;
    public float salvageEquippedTime;
    float salvagePercent;
    public Image salvageBar;
    private void Start()
    {
        if (slot is EquipmentSlot)
        {
            isEquipmentSlot = true;
        }
    }

    void Update()
    {
        float speed = isEquipmentSlot ? salvageEquippedTime : salvageSpeed;
        if (salvagable && Input.GetKey(KeyCode.F))
        {
            salvagePercent += Time.unscaledDeltaTime / speed;
            salvageBar.fillAmount = salvagePercent;
            if (salvagePercent >= 1) { 
                Inventory.Instance.Salvage(slot);
                Destroy(gameObject);
            }
            
        }
        else { salvagePercent = 0; salvageBar.fillAmount = salvagePercent; }
    }
}
