using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Color baseColor;
    public Sprite baseSprite;

    public Image icon;

    public Sprite[] frames;
    public Image frame;


    public bool locked;
    public Sprite lockedSprite;
    public Item item;

    public bool low;

    public void Refresh()
    {
        if (this is EquipmentSlot)
        {
            if (item != null)
            {
                icon.sprite = item.icon;
                icon.enabled = true;
                EquipmentSlot equipSlot = this as EquipmentSlot;
                Equipment equip = item as Equipment;
                frame.sprite = frames[(int)equip.equipType];
                frame.color = RandomizeEquipment.Instance.typeColor[(int)equipSlot.allowed];
                ChangePHIcon(0);
            }
            else
            {
                item = null;
                icon.sprite = null;
                icon.enabled = false;
                EquipmentSlot equipSlot = this as EquipmentSlot;
                frame.sprite = frames[(int)equipSlot.allowed];
                frame.color = RandomizeEquipment.Instance.typeColor[(int)equipSlot.allowed];
                ChangePHIcon();
            }
        }
        else
        {
            if (item != null)
            {
                icon.sprite = item.icon;
                icon.enabled = true;
                Equipment equip = item as Equipment;
                frame.sprite = frames[(int)equip.equipType];
                frame.color = item.color;
            }
            else
            {
                item = null;
                icon.sprite = null;
                icon.enabled = false;
                frame.sprite = baseSprite;
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

    public void ChangePHIcon()
    {
        EquipmentSlot equipSlot = this as EquipmentSlot;
        if (locked)
        {
            equipSlot.placeholderIcon.sprite = lockedSprite;
            equipSlot.placeholderIcon.color = new Color(1, 1, 1, 1);
            return;
        }
        equipSlot.placeholderIcon.color = new Color(1, 1, 1, 1);
        equipSlot.placeholderIcon.sprite = equipSlot.pHIcons[(int)equipSlot.allowed];
    }
    public void ChangePHIcon(int sprite)
    {
        EquipmentSlot equipSlot = this as EquipmentSlot;
        if (sprite == 0) { equipSlot.placeholderIcon.color = new Color(1,1,1,0); return; }
        equipSlot.placeholderIcon.color = new Color(1, 1, 1, 1);
        equipSlot.placeholderIcon.sprite = equipSlot.pHIcons[sprite];
    }

    public void Unlock()
    {
        locked = false;
        ChangePHIcon();
    }
}
