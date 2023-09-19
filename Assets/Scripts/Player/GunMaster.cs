using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMaster : MonoBehaviour
{
    public GunController[] gunArray;
    private void Awake()
    {
        gunArray = GetComponentsInChildren<GunController>();
    }
}
