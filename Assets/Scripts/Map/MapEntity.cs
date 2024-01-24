using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEntity : MonoBehaviour
{
    public GameObject representing;
    MapManager map;
    public MapEntityType entityType;
    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
        if (representing) 
            transform.position = map.mapPivot.position + (map.scaleSize * map.screensizeScale * (representing.transform.position - map.playerPos.position));
        GetComponent<Image>().enabled = true;
    }
    private void FixedUpdate()
    {
        if (representing == null) Destroy(gameObject);
        else transform.position = map.mapPivot.position + (map.scaleSize * map.screensizeScale * (representing.transform.position - map.playerPos.position));
    }
}
