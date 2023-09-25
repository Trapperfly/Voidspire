using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsResources : MonoBehaviour
{
    SpawnResourceOnDestroy resourceSpawner;
    [HideInInspector] public bool noDrop = true;
    [SerializeField] float value;
    [SerializeField] float size;
    [SerializeField] bool valueFromSize;
    private void Awake()
    {
        resourceSpawner = GameObject.Find("SpawnResourceHandler").GetComponent<SpawnResourceOnDestroy>();
    }
    private void OnDestroy()
    {
        if (noDrop)
        {
            return;
        }
        else
        {
            if (valueFromSize)
                resourceSpawner.SpawnResources(transform.localScale.x, transform.position, transform.localScale.x);
            else resourceSpawner.SpawnResources(value, transform.position, size);
        }
    }
}
