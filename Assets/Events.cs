using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Events : MonoBehaviour
{
    public delegate void OnKill();
    public OnKill onKillCallback;

    public delegate void OnHit();
    public OnKill onHitCallback;

    private void Start()
    {
        //events.onKillCallback += OnKillEvent;
        //events.onHitCallback += OnHitEvent;
    }

    public virtual void OnHitEvent(float damage, Vector2 position)
    {
        Debug.Log(damage + " damage was dealt");
        TMP_Text textAsset = Instantiate(EventsVars.instance.dmgNrGO, position, new Quaternion(), EventsVars.instance.parent).GetComponent<TMP_Text>();
        textAsset.text = damage.ToString("F1");
        onHitCallback?.Invoke();
    }
    public virtual void OnKillEvent()
    {
        Debug.Log("Something died");
        onKillCallback?.Invoke();
    }

    public virtual void OnPickUpWeapon()
    {
        Debug.Log("Picked up a weapon");
    }

    public virtual void OnPickUpResource()
    {
        Debug.Log("Picked up a resource");
    }

    //private void Awake()
    //{
    //    if (events != null && events != this)
    //    {
    //        Destroy(this);
    //    }
    //    else
    //    {
    //        events = this;
    //    }
    //}
}
