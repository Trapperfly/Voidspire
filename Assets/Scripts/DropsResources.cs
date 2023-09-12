using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsResources : MonoBehaviour
{
    SpawnResourceOnDestroy resourceSpawner;
    [HideInInspector] public bool noDrop = true;
    private void Awake()
    {
        resourceSpawner = GameObject.Find("SpawnResourceHandler").GetComponent<SpawnResourceOnDestroy>();
    }
    private void OnDestroy()
    {
        if (noDrop)
        {
            return;
        } else
            resourceSpawner.SpawnResources(transform.localScale.x, transform.position);
    }
}
