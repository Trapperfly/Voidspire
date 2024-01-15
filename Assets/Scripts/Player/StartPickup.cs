using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartPickup : MonoBehaviour
{
    public float pickupSpeedTo;
    public float pickupSpeedBack;
    public int pickupAmount;
    public int wallet;
    [SerializeField] float pickupRange;
    public GameObject grabberPrefab;
    public List<GameObject> queue = new List<GameObject>();
    GameObject pickup;
    [SerializeField] GameObject grabberHandler;
    [SerializeField] SpawnResourceOnDestroy resourceHandler;

    [SerializeField] Slider pickupSpeedToSlider;
    [SerializeField] Slider pickupSpeedBackSlider;
    [SerializeField] Slider pickupAmountSlider;
    [SerializeField] Slider pickupRangeSlider;
    [SerializeField] Slider resourceAmountSlider;

    private void Awake()
    {
        StartCoroutine(SetSliders());
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
        GetComponent<CircleCollider2D>().radius = pickupRange;
        if (grabberHandler.transform.childCount < pickupAmount && queue.Count > 0)
        {
            GameObject grabber = Instantiate(grabberPrefab, grabberHandler.transform);
            grabber.name = ("Grabber" + queue[0].GetInstanceID());
            StartCoroutine(grabber.GetComponent<Grabber>().GoToTarget(queue[0], queue[0].GetInstanceID(), pickupSpeedTo, pickupSpeedBack, pickupRange));
            queue.RemoveAt(0);
        }
    }

    public void ChangeSlider(string whatToChange)
    {
        switch (whatToChange)
        {
            case "pickupSpeedTo":
                ChangePickupSpeedTo();
                break;
            case "pickupSpeedBack":
                ChangePickupSpeedBack();
                break;
            case "pickupAmount":
                ChangePickupAmount();
                break;
            case "pickupRange":
                ChangePickupRange();
                break;
            case "resourceAmount":
                ChangeResourceAmount();
                break;
            default:
                Debug.Log(whatToChange + " is not a valid slider");
                break;
        }
    }
    void ChangePickupSpeedTo()
    {
        pickupSpeedTo = pickupSpeedToSlider.value;
        pickupSpeedToSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = pickupSpeedToSlider.value.ToString("F2");
    }
    void ChangePickupSpeedBack()
    {
        pickupSpeedBack = pickupSpeedBackSlider.value;
        pickupSpeedBackSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = pickupSpeedBackSlider.value.ToString("F2");
    }
    void ChangePickupAmount()
    {
        pickupAmount = (int)pickupAmountSlider.value;
        pickupAmountSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = pickupAmountSlider.value.ToString("F0");
    }
    void ChangePickupRange()
    {
        pickupRange = pickupRangeSlider.value;
        pickupRangeSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = pickupRangeSlider.value.ToString("F1");
    }
    void ChangeResourceAmount()
    {
        resourceHandler.amountScalar = (int)resourceAmountSlider.value;
        resourceAmountSlider.transform.GetChild(0).GetComponent<TMP_Text>().text = resourceAmountSlider.value.ToString("F0");
    }

    IEnumerator SetSliders()
    {
        ChangePickupSpeedTo();
        ChangePickupSpeedBack();
        ChangePickupAmount();
        ChangePickupRange();
        ChangeResourceAmount();
        yield return null;
    }
}
