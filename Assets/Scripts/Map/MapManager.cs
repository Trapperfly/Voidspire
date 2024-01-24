using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapEntityType
{
    Enemy,
    Neutral,
    Ally,
    Distress,
    Anomaly,
    Conflict,
    Construct,
    Quest
} 

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject neutral;
    [SerializeField] GameObject ally;
    [SerializeField] GameObject distress;
    [SerializeField] GameObject anomaly;
    [SerializeField] GameObject conflict;
    [SerializeField] GameObject construct;
    [SerializeField] GameObject quest;

    [SerializeField] Transform MapCanvas;
    [SerializeField] Transform entityHandler;
    public List<GameObject> entities;
    public Transform playerPos;
    public Transform mapPivot;
    public float scaleSize;
    [HideInInspector] public float screensizeScale;

    private void Awake()
    {
        mapPivot = GameObject.FindGameObjectWithTag("MapPivot").transform;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (screensizeScale == MapCanvas.localScale.x)
        {

        }
        else screensizeScale = MapCanvas.localScale.x;
    }
    IEnumerator UpdateMap()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject entity in entities)
        {
            ManageEntity(entity);
        }
        yield return null;
    }

    void ManageEntity(GameObject entity)
    {
        if (entity != null)
        {
            Debug.Log("Couldn't find entity");
        }
        else
        {

        }
    }

    public void AddEntity(GameObject entity, MapEntityType entityType, Vector3 position)
    {
        entities.Add(entity);
        GameObject _ = null;


        switch (entityType)
        {
            case MapEntityType.Enemy:
                _ = Instantiate(enemy, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Neutral:
                _ = Instantiate(neutral, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Ally:
                _ = Instantiate(ally, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Distress:
                _ = Instantiate(distress, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Anomaly:
                _ = Instantiate(anomaly, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Conflict:
                _ = Instantiate(conflict, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Construct:
                _ = Instantiate(construct, position, new Quaternion(), entityHandler);
                break;
            case MapEntityType.Quest:
                _ = Instantiate(quest, position, new Quaternion(), entityHandler);
                break;
            default:
                Debug.Log("Unrecognized entity type: " + entityType);
                break;
        }
        if (_ != null) _.GetComponent<MapEntity>().representing = entity;
    }

    public void RemoveEntity(GameObject entity)
    {
        entities.Remove(entity);
    }
}
