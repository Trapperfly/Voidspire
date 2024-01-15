using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events events;

    public delegate void OnKill();
    public OnKill onKillCallback;

    private void Start()
    {
        events.onKillCallback += OnKillEvent;
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

    private void Awake()
    {
        if (events != null && events != this)
        {
            Destroy(this);
        }
        else
        {
            events = this;
        }
    }
}
