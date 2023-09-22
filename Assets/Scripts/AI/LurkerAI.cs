using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkerAI : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    [SerializeField] float fleeModifier;
    [SerializeField] float combatModifier;
    [SerializeField] float repairModifier;
    [SerializeField] float repairSpeed;
    [SerializeField] float detectionRange;
    [SerializeField] float combatDetectionRange;
    [SerializeField] Transform combatTarget;
    public List<Vector2> dirs = new List<Vector2>();

    [SerializeField] ActiveTarget target;

    Vector2 dir;
    Vector2 targetPos;
    [SerializeField] LayerMask avoidMask;
    [SerializeField] LayerMask targetMask;
    Rigidbody2D rb;
    Collider2D col;
    Transform player;
    Damagable healthModule;
    float lastFrameHealth;

    float currTime;

    public bool lowHealth;
    public bool inCombat;

    float _distance;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        StartCoroutine(nameof(CheckProximity));
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        healthModule = GetComponent<Damagable>();
        lastFrameHealth = healthModule.currentHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPos = player.position;
    }
    private void FixedUpdate()
    {
        if (lowHealth && Vector2.Distance(transform.position, player.position) > 10f) RepairMode();
        else if (lowHealth) FleeMode();
        else if (inCombat) CombatMode();
        else ExplorationMode();

        if (!lowHealth)
        {
            if (healthModule.currentHealth <= healthModule.startHealth / 5)
            {
                lowHealth = true;
            }
        }
        if (healthModule.damageTaken)
        {
            if (!inCombat && !lowHealth)
            {
                Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, combatDetectionRange, targetMask);
                foreach (Collider2D target in potentialTargets)
                {
                    if (target.CompareTag("Player") && player.GetComponent<GunMaster>().hasFired == true)
                        combatTarget = target.transform;
                }
                target.target = combatTarget;
            }
            healthModule.damageTaken = false;
            inCombat = true;
            currTime = Time.time;
        }


        if (currTime <= Time.time - 10f || lowHealth)
        {
            inCombat = false;
            combatTarget = null;
            target.target = null;
        }
        lastFrameHealth = healthModule.currentHealth;
    }
    private void Update()
    {
        foreach  (Vector2 line in dirs)
        {
            Debug.DrawRay(transform.position, line.normalized);
        }
    }

    IEnumerator CheckProximity()
    {
        while (true)
        {
            dirs.Clear();
            new WaitForSeconds(0.5f);
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, detectionRange, avoidMask);
            if (objects != null)
            {
                foreach (Collider2D avoid in objects)
                {
                    if (avoid != col)
                    {
                        _distance = Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position));
                        dirs.Add(-((avoid.ClosestPoint(transform.position) - (Vector2)transform.position) * (detectionRange + 1f - _distance)));
                        Debug.DrawLine(transform.position, avoid.ClosestPoint(transform.position));
                    }
                }
            }
            Vector2 newDirection = new Vector2(0,0);
            if (dirs.Count > 0)
                foreach (Vector2 affectingVector in dirs)
                {
                    newDirection += affectingVector;
                }
            dir = newDirection;
            if (dirs.Count > 0)
                targetPos = (Vector2)transform.position + (dir * 10);
            else if (!lowHealth) targetPos = player.position;
            Debug.DrawRay(transform.position, dir);
            yield return null;
        }
    }
    IEnumerator RandomNewTarget()
    {
        while(true)
        {
            new WaitForSeconds(5f);
            yield return null;
        }
    }
    void FleeMode()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-rotSpeed * fleeModifier * rotateAmount * (detectionRange + 1f - _distance), ForceMode2D.Force);
        //rb.angularVelocity = -rotSpeed * (detectionRange + 1f - _distance) * rotateAmount;
        rb.AddForce(speed * fleeModifier * transform.up, ForceMode2D.Force);
        //rb.velocity = speed * transform.up;
    }
    void CombatMode ()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-rotSpeed * combatModifier * rotateAmount * (detectionRange + 1f - _distance), ForceMode2D.Force);
        //rb.angularVelocity = -rotSpeed * (detectionRange + 1f - _distance) * rotateAmount;
        rb.AddForce(speed * combatModifier * transform.up, ForceMode2D.Force);
        //rb.velocity = speed * transform.up;
    }
    void ExplorationMode ()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-rotSpeed * rotateAmount * (detectionRange + 1f - _distance), ForceMode2D.Force);
        //rb.angularVelocity = -rotSpeed * (detectionRange + 1f - _distance) * rotateAmount;
        rb.AddForce(speed * transform.up, ForceMode2D.Force);
        //rb.velocity = speed * transform.up;
    }
    void RepairMode ()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-rotSpeed * repairModifier * rotateAmount * (detectionRange + 1f - _distance), ForceMode2D.Force);
        //rb.angularVelocity = -rotSpeed * (detectionRange + 1f - _distance) * rotateAmount;
        rb.AddForce(speed * repairModifier * transform.up, ForceMode2D.Force);
        //rb.velocity = speed * transform.up;

        if (healthModule.currentHealth < healthModule.startHealth)
        {
            healthModule.currentHealth += repairSpeed;
            if (healthModule.currentHealth > healthModule.startHealth)
                healthModule.currentHealth = healthModule.startHealth;
        }
        else lowHealth = false;
    }
}
