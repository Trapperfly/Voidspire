using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkerAI : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    float currentModifier = 1f;
    [SerializeField] float exploreModifier;
    [SerializeField] float fleeModifier;
    [SerializeField] float combatModifier;
    [SerializeField] float repairModifier;
    [SerializeField] float repairSpeed;
    bool repairing;
    [SerializeField] float detectionRange;
    [SerializeField] float combatDetectionRange;
    Transform combatTarget;
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
        if (lowHealth && Vector2.Distance(transform.position, player.position) > 10f)
        {
            currentModifier = repairModifier;
            repairing = true;
        }
        else if (lowHealth) currentModifier = fleeModifier;
        else if (inCombat) currentModifier = combatModifier;
        else currentModifier = exploreModifier;

        AIStep();
        
        //Low health check
        if (!lowHealth)
        {
            if (healthModule.currentHealth <= healthModule.startHealth / 5)
            {
                lowHealth = true;
            }
        }

        //Get target when hit
        if (healthModule.damageTaken)
        {
            if (!inCombat && !lowHealth)
            {
                Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, combatDetectionRange, targetMask);
                foreach (Collider2D target in potentialTargets)
                {
                    if (target != col)
                    {
                        if (combatTarget == null || !combatTarget.CompareTag("Player"))
                        {
                            if (target.CompareTag("Player") && player.GetComponent<GunMaster>().hasFired == true)
                                combatTarget = target.transform;
                            else combatTarget = target.transform;
                        }
                    }
                }
                target.target = combatTarget;
            }
            healthModule.damageTaken = false;
            inCombat = true;
        }
        if (lowHealth || combatTarget == null)
        {
            inCombat = false;
            combatTarget = null;
            target.target = null;
        }
        lastFrameHealth = healthModule.currentHealth;
    }

    //Draw debug rays for vectors in CheckProximity
    private void Update()
    {
        foreach  (Vector2 line in dirs)
        {
            Debug.DrawRay(transform.position, line.normalized);
        }
    }

    //Checks for objects in proximity when navigating
    IEnumerator CheckProximity()
    {
        while (true)
        {
            //Clear targets from last iteration
            dirs.Clear();
            new WaitForSeconds(0.5f);
            //Check for colliders in a radius.
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, detectionRange, avoidMask);
            if (objects != null)
            {
                foreach (Collider2D avoid in objects)
                {
                    if (avoid != col)
                    {
                        //Calculates negative distance vectors from the objects, and add these to a list
                        _distance = Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position));
                        //Uses distance in the multiplication to add strength when object is close
                        dirs.Add(-((avoid.ClosestPoint(transform.position) - (Vector2)transform.position) * (detectionRange + 1f - _distance)));
                        Debug.DrawLine(transform.position, avoid.ClosestPoint(transform.position));
                    }
                }
            }
            Vector2 newDirection = new (0,0);
            //Add the vectors together to get the optimal direction to avoid most things
            if (dirs.Count > 0)
                foreach (Vector2 affectingVector in dirs)
                {
                    newDirection += affectingVector;
                }
            dir = newDirection;
            //Creates a vector to be used as a target for the AI to move to
            if (dirs.Count > 0)
                targetPos = (Vector2)transform.position + (dir * 10);
            else if (!lowHealth) targetPos = player.position;
            Debug.DrawRay(transform.position, dir);
            yield return null;
        }
    }
    void AIStep()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-rotSpeed * currentModifier * rotateAmount * (detectionRange + 1f - _distance), ForceMode2D.Force);
        rb.AddForce(speed * currentModifier * transform.up, ForceMode2D.Force);
        if (repairing)
        {
            if (healthModule.currentHealth < healthModule.startHealth)
            {
                healthModule.currentHealth += repairSpeed;
                if (healthModule.currentHealth > healthModule.startHealth)
                    healthModule.currentHealth = healthModule.startHealth;
            }
            else
            {
                repairing = false;
                lowHealth = false;
            }
        }
    }
}
