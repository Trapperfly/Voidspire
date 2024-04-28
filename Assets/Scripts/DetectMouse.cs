using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectMouse : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.click, transform.position);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hover, transform.position);
    }
}
