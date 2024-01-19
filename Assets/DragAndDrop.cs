using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, 
    IPointerDownHandler, 
    IPointerUpHandler,
    IBeginDragHandler, 
    //IEndDragHandler, 
    IDragHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler
    //IDropHandler
{
    Vector2 startPos;
    Vector2 endPos;

    InventorySlot slot0;
    InventorySlot slot1;

    bool isHovering;
    bool allowed;

    RectTransform line;

    public float time = 0.2f;
    public void OnBeginDrag(PointerEventData eventData)
    {
        slot0 = eventData.pointerDrag.GetComponent<InventorySlot>();
        Debug.Log(slot0);
        line = new GameObject().AddComponent<RectTransform>();
        line.gameObject.layer = 2;
        //line = gameObject.AddComponent<RectTransform>();
        line.SetParent(GameObject.FindGameObjectWithTag("Inventory").transform);
        line.gameObject.AddComponent<Image>().raycastTarget = false;
        line.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
        line.gameObject.AddComponent<ShrinkAndExpire>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (line != null)
        {
            endPos = eventData.position;
            line.position = Vector2.Lerp(startPos, endPos, 0.5f);
            line.localScale = new Vector2(0.1f, Vector2.Distance(startPos, endPos) / 100);

            var dir = endPos - (Vector2)line.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            line.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHovering)
        {
            startPos = eventData.position;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.GetComponentInParent<InventorySlot>());
        Debug.Log(line);
        Debug.Log(isHovering);
        if (eventData.pointerEnter.CompareTag("ItemPanel")) // && allowed
        {
            slot1 = eventData.pointerEnter.GetComponentInParent<InventorySlot>();
            bool dragged = (!slot0 || !slot1) ? false : true;
            if (dragged && slot1 is EquipmentSlot && slot0.item is Equipment)
            {
                EquipmentSlot s1 = slot1 as EquipmentSlot;
                Equipment e0 = slot0.item as Equipment;

                if (s1.allowed == e0.equipType || (int)s1.allowed == 1)
                {
                    DoSwapAndShrink();
                    return;
                }
                else { StartCoroutine(line.GetComponent<ShrinkAndExpire>().Expire()); line = null; return; }
            }
            else if (dragged && slot0 is EquipmentSlot && slot1.item is Equipment)
            {
                EquipmentSlot s0 = slot0 as EquipmentSlot;
                Equipment e1 = slot1.item as Equipment;

                if (s0.allowed == e1.equipType || (int)s0.allowed == 1)
                {
                    DoSwapAndShrink();
                    return;
                }
                else { StartCoroutine(line.GetComponent<ShrinkAndExpire>().Expire()); line = null; return; }
            }
            else if (dragged) DoSwapAndShrink();
            else Debug.LogWarning("Just clicked. Do something more here?");
        }
        else { StartCoroutine(line.GetComponent<ShrinkAndExpire>().Expire()); line = null; }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Debug.Log("hovering");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Debug.Log("not hovering");
    }

    public void SwapSpaces()
    {
        Debug.Log("Swapping " + slot0 + " and " + slot1);
        Inventory.Instance.Swap(slot0, slot1);
    }

    public void DoSwapAndShrink()
    {
        SwapSpaces();
        if (line)
        {
            line.GetComponent<ShrinkAndExpire>().startPos = startPos;
            line.GetComponent<ShrinkAndExpire>().endPos = endPos;
            line.GetComponent<ShrinkAndExpire>().enabled = true;
            StartCoroutine(line.GetComponent<ShrinkAndExpire>().Shrink());
            line = null;
        }
    }
}
