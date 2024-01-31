using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.WebRequestMethods;

public class StartPickup : MonoBehaviour
{
    public int wallet;
    [SerializeField] float pickupRange;
    public GameObject grabberPrefab;
    public List<GameObject> queue = new List<GameObject>();
    GameObject pickup;
    [SerializeField] GameObject grabberHandler;
    [SerializeField] SpawnResourceOnDestroy resourceHandler;

    EquipmentController equipment;
    Collector collector;

    private void Awake()
    {
        EquipmentController.Instance.onEquipmentLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        SetNewStats();

        EquipmentController.Instance.onEquipmentLoadComplete -= CustomStart;
        EquipmentController.Instance.onEquipmentLoadComplete += SetNewStats;
    }

    private void SetNewStats()
    {
        collector = equipment.collectorSlots[0].item as Collector;
        //Debug.Log(collector);
        GetComponent<CircleCollider2D>().radius = collector.range;
        //Debug.Log(collector);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            pickup = collision.gameObject;
            queue.Add(pickup);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            pickup = collision.gameObject;
            queue.Remove(pickup);
        }
    }
    private void FixedUpdate()
    {
        if (grabberHandler.transform.childCount < collector.amount && queue.Count > 0)
        {
            GameObject grabber = Instantiate(grabberPrefab, grabberHandler.transform);
            grabber.name = ("Grabber" + queue[0].GetInstanceID());
            StartCoroutine
                (grabber.GetComponent<Grabber>().GoToTarget
                (queue[0], queue[0].GetInstanceID(), collector.collectorSpeedTo, collector.collectorSpeedFrom, collector.range));
            queue.RemoveAt(0);
        }
    }
}
