using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsVars : MonoBehaviour
{
    public GameObject dmgNrGO;
    public Transform parent;

    public static EventsVars instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
