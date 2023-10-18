using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntity : MonoBehaviour
{
    public GameObject representing;
    MapManager map;
    public string entityType;

    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
    }
    private void FixedUpdate()
    {
        if (representing == null) Destroy(gameObject);
        else transform.position = map.mapPivot.position + ((representing.transform.position - map.playerPos.position) * map.scaleSize * map.screensizeScale);
    }
}
