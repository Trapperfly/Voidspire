using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMaster : MonoBehaviour
{
    public GunStats[] gunArray;
    public bool hasFired;
    bool hasFiredLog;
    float currTime;

    private void Awake()
    {
        gunArray = GetComponentsInChildren<GunStats>();
    }
    private void FixedUpdate()
    {
        if (hasFired && !hasFiredLog)
        {
            currTime = Time.time;
            hasFiredLog = true;
        }
        if (currTime <= Time.time - 5)
        {
            hasFiredLog = false;
            hasFired = false;
        }

    }
}
