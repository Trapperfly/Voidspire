using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.WebRequestMethods;
using System.Drawing;

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
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (!equipment.collectorSlots[0].item) 
        {
            collector = null;
            Collector placeholderCollector = ScriptableObject.CreateInstance<Collector>();
            placeholderCollector.collectorType = CollectorTypes.Default;
            placeholderCollector.amount = 1;
            placeholderCollector.range = 0.4f;
            placeholderCollector.collectorSpeedFrom = 3;
            placeholderCollector.collectorSpeedTo = 3;
            collector = placeholderCollector;
            col.radius = collector.range;
            return; 
        }
        collector = equipment.collectorSlots[0].item as Collector;
        col.radius  = collector.range;
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
        if (GlobalRefs.Instance.playerIsDead) return;
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
