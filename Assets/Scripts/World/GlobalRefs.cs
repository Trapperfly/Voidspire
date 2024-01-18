using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRefs : MonoBehaviour 
{
    public static GlobalRefs Instance;

    public GameObject player;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
