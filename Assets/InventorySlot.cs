using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Color baseColor;

    public Image icon;

    public Image frame;

    public Item item;

    public void Refresh()
    {
        if (this is EquipmentSlot)
        {
            if (item != null)
            {
                icon.sprite = item.icon;
                icon.enabled = true;
                EquipmentSlot equipSlot = this as EquipmentSlot;
                frame.color = RandomizeEquipment.Instance.typeColor[(int)equipSlot.allowed];
            }
            else
            {
                item = null;
                icon.sprite = null;
                icon.enabled = false;
                EquipmentSlot equipSlot = this as EquipmentSlot;
                frame.color = RandomizeEquipment.Instance.typeColor[(int)equipSlot.allowed];
            }
        }
        else
        {
            if (item != null)
            {
                icon.sprite = item.icon;
                icon.enabled = true;
                frame.color = item.color;
            }
            else
            {
                item = null;
                icon.sprite = null;
                icon.enabled = false;
                frame.color = baseColor;
            }
        }
        

    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        frame.color = item.color;
    }

    public void ClearSlot()
    { 
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        frame.color = baseColor;
    }
}
