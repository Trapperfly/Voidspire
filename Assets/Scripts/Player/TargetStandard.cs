using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStandard : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] GunMaster guns;
    [SerializeField] GameObject targetTransformPrefab;
    [SerializeField] Transform holder;
    ActiveTarget target;
    GameObject targetInstance;
    float targetHoldTimer;
    bool targetingDone = false;
    [SerializeField] float targetingRange = 1f;
    [SerializeField] LayerMask targetMask;
    private void Awake()
    {
        target = GetComponent<ActiveTarget>();
    }
    private void FixedUpdate()
    {
        if (GlobalRefs.Instance.playerIsDead) return;
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
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.targetEnemy, transform.position);
            Debug.Log("Hit targetable");
            targetInstance = Instantiate    //If hit nothing, make a vector 2 and use that as target
            (targetTransformPrefab, hit.transform.position, new Quaternion(), hit.transform);
            StartCoroutine(target.InitTargetValues(hit.transform, hit.GetComponent<Rigidbody2D>())); //If hit something hittable, make that the target
        }
        else
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.targetNothing, transform.position);
            Debug.Log("Spawning target transform");
            targetInstance = Instantiate    //If hit nothing, make a vector 2 and use that as target
            (
                targetTransformPrefab,
                new Vector3
                (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                0), new Quaternion()
            );
            StartCoroutine(target.InitTargetValues(targetInstance.transform));
        }
        StartCoroutine(SetTargetValues());
    }
    public IEnumerator SetTargetValues()
    {
        foreach (Transform bulletController in holder)
        {
            BulletController bc = bulletController.GetComponent<BulletController>();
            if (target.target != null)
                bc.target = target.target;
        }

        yield return null;
    }
    public void RemoveTarget()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.targetCancel, transform.position);
        if (targetInstance != null)
        {
            Destroy(targetInstance);
            StartCoroutine(target.ClearTarget());
        }
        else if (target.target != null)
            StartCoroutine(target.ClearTarget());
    }
}
