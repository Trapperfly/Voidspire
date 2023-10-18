using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntityInit : MonoBehaviour
{
    public enum MapEntityType
    {
        Player,
        Enemy,
        Neutral,
        Ally,
        Distress,
        Anomaly,
        Coflict,
        Construct,
        Quest
    }
    public MapEntityType mapEntityType;
    MapManager map;
    private void Awake()
    {
        map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
    }
    private void Start()
    {
        map.AddEntity(gameObject, mapEntityType.ToString(), transform.position);
    }
}
