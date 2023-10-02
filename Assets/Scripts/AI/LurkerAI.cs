using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkerAI : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    [SerializeField] float strafeSpeed;
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
    public bool sceptic;
    public bool seenPlayer;

    float _distance;

    Vector2 spawnPoint;
    [SerializeField] float awayFromTargetRadius = 10;

    [SerializeField] ParticleSystem ps;

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
        spawnPoint = transform.position;
        StartCoroutine(nameof(GetNewTargetPosOverTime));
    }
    private void Start()
    {
        dir = transform.up;
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
                StartCoroutine(nameof(GetNewTargetPos));
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
            StartCoroutine(nameof(GetNewTargetPos));
        }
        if (inCombat)
        {
            if (lowHealth || combatTarget == null)
            {
                inCombat = false;
                combatTarget = null;
                target.target = null;
                StartCoroutine(nameof(GetNewTargetPos));
            }
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
        ps.transform.rotation = Quaternion.RotateTowards(ps.transform.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, -dir.normalized)), 10);
        var pEmission = ps.emission;
        if (dir != new Vector2(0, 0))
        {
            pEmission.rateOverTime = (detectionRange - _distance) * 20f;
        }
        else if (!ps.isPaused)
        {
            pEmission.rateOverTime = 0;
        }

    }

    //Checks for objects in proximity when navigating
    IEnumerator CheckProximity()
    {
        while (true)
        {
            //Clear targets from last iteration
            dirs.Clear();
            new WaitForSeconds(0.2f);
            /*
            //Check for colliders in a radius.
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, detectionRange, avoidMask);
            if (objects != null)
            {
                _distance = detectionRange;
                foreach (Collider2D avoid in objects)
                {
                    if (avoid != col)
                    {
                        if (!seenPlayer && avoid.gameObject.layer == 6) //Layer 6 is Player
                            seenPlayer = true;
                        //Calculates negative distance vectors from the objects, and add these to a list
                        if (Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position)) < _distance)
                            _distance = Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position));
                        //Uses distance in the multiplication to add strength when object is close
                        dirs.Add(-((avoid.ClosestPoint(transform.position) - (Vector2)transform.position) * ((detectionRange + 1f - _distance) * 2)));
                        Debug.DrawLine(transform.position, avoid.ClosestPoint(transform.position));
                    }
                }
            }
            Vector2 newDirection = new (0,0);
            //Add the vectors together to get the optimal direction to avoid most things
            if (dirs.Count > 0)
                foreach (Vector2 affectingVector in dirs)
                {
                    newDirection = affectingVector;
                }
            dir = newDirection;
            //Creates a vector to be used as a target for the AI to move to
            Debug.DrawRay(transform.position, dir);
            */
            Vector2 newDirection = new(0, 0);
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, detectionRange, avoidMask);
            if (objects != null)
            {
                _distance = detectionRange;
                foreach (Collider2D avoid in objects)
                {
                    if (avoid != col)
                    {
                        if (!seenPlayer && avoid.gameObject.layer == 6) //Layer 6 is Player
                            seenPlayer = true;
                        if (Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position)) < _distance)
                        {
                            _distance = Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position));
                            newDirection = -(avoid.ClosestPoint(transform.position) - (Vector2)transform.position);
                        }
                        Debug.DrawRay(transform.position, dir);
                    }
                }
            }
            //Add the vectors together to get the optimal direction to avoid most things
            dir = newDirection;
            //Creates a vector to be used as a target for the AI to move to
            Debug.DrawRay(transform.position, dir);
            yield return null;
        }
    }

    IEnumerator GetNewTargetPosOverTime()
    {
        while (true)
        {
            StartCoroutine(nameof(GetNewTargetPos));
            yield return new WaitForSeconds(6f);
        }
    }
    IEnumerator GetNewTargetPos()
    {
        if (lowHealth)
        {
            targetPos = 10 * awayFromTargetRadius * (Vector2)transform.up;
        }
        else if (inCombat)
        {
            if (combatTarget != null)
                targetPos = (Vector2)combatTarget.position + Random.insideUnitCircle * (awayFromTargetRadius / 2);
        }
        else if (seenPlayer)
        {
            targetPos = (Vector2)player.position + Random.insideUnitCircle * awayFromTargetRadius;
        }
        else targetPos = spawnPoint + Random.insideUnitCircle * awayFromTargetRadius;
        yield return null;
    }
    void AIStep()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-rotSpeed * currentModifier * rotateAmount, ForceMode2D.Force);
        rb.AddForce(Mathf.Lerp(0.2f, 1f,(_distance / detectionRange)) * currentModifier * speed * transform.up, ForceMode2D.Force);
        rb.AddForce(((detectionRange + 1f - _distance) / 2) * currentModifier * strafeSpeed * dir.normalized, ForceMode2D.Force);
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
                StartCoroutine(nameof(GetNewTargetPos));
            }
        }
    }

    /*
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.3f);
    }
    */
}
