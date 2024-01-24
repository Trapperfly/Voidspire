using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntityInit : MonoBehaviour
{
    public MapEntityType mapEntityType;
    MapManager map;
    private void Awake()
    {
        map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
        map.AddEntity(gameObject, mapEntityType, transform.position);
    }
    private void Start()
    {
        
    }
}
