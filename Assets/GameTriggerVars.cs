using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTriggerVars : MonoBehaviour
{
    public GameObject dmgNrGO;
    public Transform parent;

    public static GameTriggerVars instance;

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
