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
        Debug.Log(eventData.pointerEnter);
        Debug.Log(line);
        Debug.Log(isHovering);
        if (eventData.pointerEnter.CompareTag("ItemPanel")) // && allowed
        {
            line.GetComponent<ShrinkAndExpire>().startPos = startPos;
            line.GetComponent<ShrinkAndExpire>().endPos = endPos;
            line.GetComponent<ShrinkAndExpire>().enabled = true;
            StartCoroutine(line.GetComponent<ShrinkAndExpire>().Shrink());
            line = null;
        }
        else
        {
            StartCoroutine(line.GetComponent<ShrinkAndExpire>().Expire());
            line = null;
        }
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
}
