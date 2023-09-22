using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStandard : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] GunMaster guns;
    [SerializeField] GameObject targetTransformPrefab;
    [SerializeField] BulletController bc;
    ActiveTarget target;
    GameObject targetInstance;
    float targetHoldTimer;
    bool targetingDone = false;
    [SerializeField] float targetingRange = 1f;
    [SerializeField] LayerMask targetMask;
    private void Awake()
    {
        bc = GameObject.FindGameObjectWithTag("BulletHolder").GetComponent<BulletController>();
        target = GetComponent<ActiveTarget>();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))   //Targeting system
        {
            if (targetHoldTimer >= 30 && targetingDone == false)
            {
                SetTarget();
                targetingDone = true;
            }
            if (targetingDone == false)
                targetHoldTimer += 1;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (targetHoldTimer < 30)
                RemoveTarget();
            targetHoldTimer = 0;
            targetingDone = false;
        }
    }
    void SetTarget()
    {
        Debug.Log("Mouse 1 clicked");
        if (targetInstance != null)
            Destroy(targetInstance);    //Remove previous target
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Collider2D hit = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(ray.origin, targetingRange, targetMask);
        float bestDistance = 10f;
        foreach (Collider2D potentialHit in hits)
        {
            float _distance = Vector2.Distance(ray.origin, potentialHit.ClosestPoint(ray.origin));
            Debug.Log(_distance);
            if (_distance == 0)
            {
                hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity).collider;
                bestDistance = 0;
            }
            else if (_distance < bestDistance)
            {
                bestDistance = _distance;
                hit = potentialHit;
            }
        }
        Debug.Log("Chose: " + bestDistance);
        if (hit != null)
        {
            Debug.Log("Hit targetable");
            targetInstance = Instantiate    //If hit nothing, make a vector 2 and use that as target
            (targetTransformPrefab, hit.transform.position, new Quaternion(), hit.transform);
            target.target = hit.transform;   //If hit something hittable, make that the target
        }
        else
        {
            Debug.Log("Spawning target transform");
            targetInstance = Instantiate    //If hit nothing, make a vector 2 and use that as target
            (
                targetTransformPrefab,
                new Vector3
                (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                0), new Quaternion()
            );
            target.target = targetInstance.transform;
        }
        StartCoroutine(SetTargetValues());
    }
    public IEnumerator SetTargetValues()
    {
        if (target.target != null)
            bc.target = target.target;
        yield return null;
    }
    public void RemoveTarget()
    {
        if (targetInstance != null)
        {
            Destroy(targetInstance);
            target.target = null;
        }
        else if (target.target != null)
            target.target = null;
    }
}
