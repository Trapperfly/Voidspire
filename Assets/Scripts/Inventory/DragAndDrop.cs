using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, 
    IPointerDownHandler, 
    IPointerUpHandler,
    IBeginDragHandler, 
    IEndDragHandler,
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
    List<RectTransform> rects = new();

    public float time = 0.2f;

    public GameObject infoBox;
    public bool infoBoxActive;
    public GameObject activeInfoBox;

    DragAndDropMaster dnd;

    Canvas canvas;
    private void Start()
    {
        dnd = DragAndDropMaster.Instance;
        canvas = transform.root.GetComponent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dnd.slot = transform;
        slot0 = eventData.pointerDrag.GetComponent<InventorySlot>();
        //Debug.Log(slot0);
        line = new GameObject().AddComponent<RectTransform>();
        rects.Add(line);
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
            //Debug.Log("Screen resolution is " + 1920f/Screen.width + "x" + 1080f/Screen.height);
            line.localScale = new Vector2(0.1f, Vector2.Distance(startPos, endPos) / canvas.scaleFactor / 100f);

            var dir = endPos - (Vector2)line.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            line.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
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
        //Debug.Log(eventData.pointerEnter.GetComponentInParent<InventorySlot>());
        //Debug.Log(line);
        //Debug.Log(isHovering);
        dnd.slot = null;
        Destroy(activeInfoBox);
        activeInfoBox = null;
        Destroy(dnd.savedInfoBox);
        dnd.savedInfoBox = null;
        if (eventData.pointerEnter.CompareTag("ItemPanel")) // && allowed
        {
            slot1 = eventData.pointerEnter.GetComponentInParent<InventorySlot>();

            Destroy(eventData.pointerEnter.GetComponentInParent<DragAndDrop>().activeInfoBox);

            bool dragged = slot0 && slot1;
            if (dragged && slot1 is EquipmentSlot && slot0.item is Equipment)
            {
                EquipmentSlot s1 = slot1 as EquipmentSlot;
                Equipment e0 = slot0.item as Equipment;
                if (s1.locked && (slot0.item as Equipment).equipType == EquipmentTypes.Key)
                {
                    Inventory.Instance.Unlock(slot0, s1);
                    DoShrink();
                    return;
                }
                if (s1.allowed == e0.equipType || (int)s1.allowed == 1)
                {
                    DoSwapAndShrink();
                    eventData.pointerEnter.GetComponentInParent<DragAndDrop>().DisplayInfoBox(eventData.pointerEnter.transform.parent.parent, false);
                    return;
                }
                else { StartCoroutine(line.GetComponent<ShrinkAndExpire>().Expire()); line = null; return; }
            }
            else if (dragged && slot0 is EquipmentSlot && slot1.item is Equipment)
            {
                EquipmentSlot s0 = slot0 as EquipmentSlot;
                Equipment e1 = slot1.item as Equipment;
                if (s0.locked && (slot1.item as Equipment).equipType == EquipmentTypes.Key)
                {
                    Inventory.Instance.Unlock(slot1, s0);
                    DoShrink();
                    return;
                }
                if (s0.allowed == e1.equipType || (int)s0.allowed == 1)
                {
                    DoSwapAndShrink();
                    eventData.pointerEnter.GetComponentInParent<DragAndDrop>().DisplayInfoBox(eventData.pointerEnter.transform.parent.parent, false);
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
        if (eventData.dragging && GetComponent<InventorySlot>().item && dnd.slot && dnd.slot.GetComponent<InventorySlot>().item)
        {
            //Debug.Log("Displaying ol box");
            DisplayOldInfoBox(dnd.slot, false);
        }
        //Debug.Log(eventData.pointerEnter.transform.parent.parent.ToString());
        //Debug.Log(GetComponent<InventorySlot>().item);
        isHovering = true;
        if (eventData.dragging && GetComponent<InventorySlot>().item)
        {
            DisplayInfoBox(transform, false);
        }
        else if (GetComponent<InventorySlot>().item)
        {
            DisplayInfoBox(transform, true);
        }
        //Debug.Log("hovering");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (eventData.dragging)
        {
            Destroy(activeInfoBox);
            Destroy(dnd.savedInfoBox);
            dnd.savedInfoBox = null;
            activeInfoBox = null;
            infoBoxActive = false;
        }
        else
        {
            Destroy(activeInfoBox);
            activeInfoBox = null;
            infoBoxActive = false;
        }
        //Debug.Log("not hovering");
    }

    public void SwapSpaces()
    {
        //Debug.Log("Swapping " + slot0 + " and " + slot1);
        Inventory.Instance.Swap(slot0, slot1);
    }

    public void DoSwapAndShrink()
    {
        SwapSpaces();
        if (line)
        {
            ShrinkAndExpire ls = line.GetComponent<ShrinkAndExpire>();
            ls.startPos = startPos;
            ls.endPos = endPos;
            ls.canvasScale = canvas.scaleFactor;
            ls.enabled = true;
            StartCoroutine(ls.Shrink());
            line = null;
        }
    }

    public void DoShrink()
    {
        if (line)
        {
            ShrinkAndExpire ls = line.GetComponent<ShrinkAndExpire>();
            ls.startPos = startPos;
            ls.endPos = endPos;
            ls.canvasScale = canvas.scaleFactor;
            ls.enabled = true;
            StartCoroutine(ls.Shrink());
            line = null;
        }
    }

    private void OnDisable()
    {
        if (line)
        {
            rects.Remove(line);
            Destroy(line.gameObject);
            line = null;
        }
        if (rects.Count > 0 ) { foreach (RectTransform rect in rects) { if (rect) { Destroy(rect.gameObject); } } }
        Destroy(activeInfoBox);
        Destroy(dnd.savedInfoBox);
        dnd.savedInfoBox = null;
        activeInfoBox = null;
        dnd.slot = null;
    }

    private void DisplayInfoBox(Transform slot, bool salvagable)
    {
        activeInfoBox =
                Instantiate(infoBox,
                (Vector2)slot.transform.position + new Vector2(55, 55),
                new Quaternion(),
                InventoryUI.Instance.inventoryGraphicsParent);

        activeInfoBox.GetComponent<HoldToSalvage>().salvagable = salvagable;
        activeInfoBox.GetComponent<HoldToSalvage>().slot = slot.GetComponent<InventorySlot>();

        Equipment sItem = slot.GetComponent<InventorySlot>().item as Equipment;
        Image backPanel = activeInfoBox.transform.GetChild(0).GetComponent<Image>();
        Image frontPanel = activeInfoBox.transform.GetChild(1).GetComponent<Image>();
        Image deleteBack = activeInfoBox.transform.GetChild(2).GetComponent<Image>();
        Image deleteFront = activeInfoBox.transform.GetChild(3).GetComponent<Image>();

        int addSpace = salvagable ? 1 : 0;
        frontPanel.rectTransform.sizeDelta =
            new Vector2(frontPanel.rectTransform.sizeDelta.x, frontPanel.rectTransform.sizeDelta.y + (26 * (sItem.statLength + addSpace)));
        backPanel.rectTransform.sizeDelta =
            new Vector2(backPanel.rectTransform.sizeDelta.x, backPanel.rectTransform.sizeDelta.y + (26 * (sItem.statLength + addSpace)));
        deleteBack.transform.localPosition -= new Vector3(0, 26 * (sItem.statLength + addSpace), 0);
        deleteFront.transform.localPosition -= new Vector3(0, 26 * (sItem.statLength + addSpace), 0);

        backPanel.color = sItem.color;
        TMP_Text[] texts = activeInfoBox.GetComponentsInChildren<TMP_Text>();
        texts[6].transform.localPosition -= new Vector3(0, 26 * (sItem.statLength + addSpace), 0);
        texts[0].text = sItem.itemName;
        SetItemDescriptionText(texts[1], sItem);
        texts[2].text = sItem.statsText;
        texts[3].text = sItem.statsValues;
        texts[4].text = sItem.level.ToString();

        if (!salvagable)
        {
            deleteBack.gameObject.SetActive(false);
            deleteFront.gameObject.SetActive(false);
            texts[6].gameObject.SetActive(false);
        }
        infoBoxActive = true;
    }

    private void DisplayOldInfoBox(Transform slot, bool salvagable)
    {
        dnd.savedInfoBox =
                Instantiate(infoBox,
                (Vector2)slot.transform.position + new Vector2(55, 55),
                new Quaternion(),
                InventoryUI.Instance.inventoryGraphicsParent);

        dnd.savedInfoBox.GetComponent<HoldToSalvage>().salvagable = salvagable;
        dnd.savedInfoBox.GetComponent<HoldToSalvage>().slot = slot.GetComponent<InventorySlot>();

        Equipment sItem = slot.GetComponent<InventorySlot>().item as Equipment;
        Image backPanel = dnd.savedInfoBox.transform.GetChild(0).GetComponent<Image>();
        Image frontPanel = dnd.savedInfoBox.transform.GetChild(1).GetComponent<Image>();
        Image deleteBack = dnd.savedInfoBox.transform.GetChild(2).GetComponent<Image>();
        Image deleteFront = dnd.savedInfoBox.transform.GetChild(3).GetComponent<Image>();

        int addSpace = salvagable ? 1 : 0;
        frontPanel.rectTransform.sizeDelta =
            new Vector2(frontPanel.rectTransform.sizeDelta.x, frontPanel.rectTransform.sizeDelta.y + (26 * (sItem.statLength + addSpace)));
        backPanel.rectTransform.sizeDelta =
            new Vector2(backPanel.rectTransform.sizeDelta.x, backPanel.rectTransform.sizeDelta.y + (26 * (sItem.statLength + addSpace)));
        deleteBack.transform.localPosition -= new Vector3(0, 26 * (sItem.statLength + addSpace), 0);
        deleteFront.transform.localPosition -= new Vector3(0, 26 * (sItem.statLength + addSpace), 0);

        backPanel.color = sItem.color;
        TMP_Text[] texts = dnd.savedInfoBox.GetComponentsInChildren<TMP_Text>();
        texts[6].transform.localPosition -= new Vector3(0, 26 * (sItem.statLength + addSpace), 0);
        texts[0].text = sItem.itemName;
        SetItemDescriptionText(texts[1], sItem);
        texts[2].text = sItem.statsText;
        texts[3].text = sItem.statsValues;
        texts[4].text = sItem.level.ToString();

        if (!salvagable)
        {
            deleteBack.gameObject.SetActive(false);
            deleteFront.gameObject.SetActive(false);
            texts[6].gameObject.SetActive(false);
        }
    }
    void SetItemDescriptionText(TMP_Text textasset,Equipment equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentTypes.None:
                break;
            case EquipmentTypes.All:
                break;
            case EquipmentTypes.Weapon:
                textasset.text = (equipment as Weapon).weaponType.ToString() + " - Weapon";
                break;
            case EquipmentTypes.Shield:
                textasset.text = (equipment as Shield).shieldType.ToString() + " - Shield";
                break;
            case EquipmentTypes.Thruster:
                textasset.text = (equipment as Thrusters).stlType.ToString() + " - Thruster";
                break;
            //case EquipmentTypes.FTL:
            //    textasset.text = (equipment as FTLEngine).ftlType.ToString() + " - FTL";
            //    break;
            case EquipmentTypes.Hull:
                textasset.text = (equipment as Hull).hullType.ToString() + " - Hull";
                break;
            case EquipmentTypes.Scanner:
                textasset.text = (equipment as Scanner).type.ToString() + " - Scanner";
                break;
            case EquipmentTypes.Cargo:
                textasset.text = " - Cargo";
                break;
            case EquipmentTypes.Collector:
                textasset.text = (equipment as Collector).collectorType.ToString() + " - Collect";
                break;
            case EquipmentTypes.Relic:
                textasset.text = "Relic";

                break;
            case EquipmentTypes.Default:
                break;
            default:
                break;
        }
    }
}
