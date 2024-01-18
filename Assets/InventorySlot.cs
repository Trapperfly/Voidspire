using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Color color;

    public Image icon;

    public Item item;

    public void Refresh()
    {
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        } else
        {
            icon.sprite = null;
            icon.enabled = false;
        }

    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    { 
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
