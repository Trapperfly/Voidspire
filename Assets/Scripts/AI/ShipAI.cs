using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;


public class ShipAI : MonoBehaviour
{
    public Ship ship;
    float currentModifier = 1f;
    bool repairing;
    bool firingSpecial;
    bool rotate = true;
    public bool jumping;
    public Transform aiBullets;

    public AIGun[] guns;

    public int curCD = 0;

    Transform combatTarget;
    public List<Vector2> dirs = new List<Vector2>();

    [SerializeField] ActiveTarget target;
    Transform targetTransform;

    Vector2 dir;
    Vector2 targetPos;
    [SerializeField] LayerMask avoidMask;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask pickupMask;
    Rigidbody2D rb;
    Collider2D col;
    Damagable healthModule;
    float lastFrameHealth;

    float currTime;

    public bool lowHealth;
    public bool inCombat;
    public bool sceptic;
    public bool seenPlayer;
    public bool idle;

    float _distance;

    Vector2 spawnPoint;

    [SerializeField] ParticleSystem ps;
    [SerializeField] ParticleSystem thrustersPS;

    int jumpTimer = 0;
    bool isEnraged = false;

    private void Start()
    {
        var tEmis = thrustersPS.emission;
        tEmis.enabled = false;
        aiBullets = GameObject.FindGameObjectWithTag("AIBulletHolder").transform;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        StartCoroutine(nameof(CheckProximity));

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        healthModule = GetComponent<Damagable>();
        healthModule.startHealth = ship.maxHealth;
        healthModule.currentHealth = ship.maxHealth;
        lastFrameHealth = healthModule.currentHealth;
        targetPos = GlobalRefs.Instance.player.transform.position;
        spawnPoint = transform.position;
        StartCoroutine(nameof(GetNewTargetPosOverTime));
        dir = transform.up;
    }
    private void FixedUpdate()
    {
        currentModifier = CheckDistanceAndSetModifier();

        TryStopCombat();

        if (lowHealth)
        {
            switch (ship.lowHealthBehaviour)
            {
                case LowHealthBehaviour.None:
                    break;
                case LowHealthBehaviour.Flee:
                    NormalMove();
                    break;
                case LowHealthBehaviour.Jump:
                    Strafe();
                    Jump();
                    Rotate();
                    break;
                case LowHealthBehaviour.DropLootAndJump:
                    Strafe();
                    Jump();
                    Rotate();
                    break;
                case LowHealthBehaviour.Surrender:
                    //ContactPlayer
                    break;
                case LowHealthBehaviour.SurrenderButBargain:
                    //ContactPlayer
                    break;
                case LowHealthBehaviour.SurrenderButTrick:
                    //ContactPlayer
                    break;
                case LowHealthBehaviour.Enrage:
                    NormalMove();
                    isEnraged = true;
                    //if (isEnraged)
                    //{
                    //    ship.fireRate *= 2;
                    //}
                    break;
                case LowHealthBehaviour.SelfDestruct:
                    NormalMove();
                    break;
                default:
                    break;
            }
        }
        else if (inCombat)
        {
            switch (ship.combatBehaviour)
            {
                case CombatBehaviour.None:
                    break;
                case CombatBehaviour.StandGroundAndAttack:
                    NormalMove();
                    break;
                case CombatBehaviour.RotateTowardsAndAttack:
                    Rotate();
                    Strafe();
                    Repair();
                    break;
                case CombatBehaviour.ChaseAndAttack:
                    NormalMove();
                    break;
                case CombatBehaviour.RunAndAttack:
                    NormalMove();
                    break;
                case CombatBehaviour.SelfDestruct:
                    NormalMove();
                    break;
                case CombatBehaviour.Crash:
                    NormalMove();
                    break;
                case CombatBehaviour.JumpImmediately:
                    Strafe();
                    Rotate();
                    Jump();
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (ship.behaviour)
            {
                case Behaviour.None:
                    break;
                case Behaviour.NotDefined:
                    NormalMove();
                    break;
                case Behaviour.TargetResources:
                    NormalMove();
                    break;
                case Behaviour.StalkTarget:
                    NormalMove();
                    break;
                case Behaviour.JumpOnSight:
                    if (seenPlayer) { Strafe(); Rotate(); Jump(); break; }
                    NormalMove();
                    break;
                case Behaviour.RepairTarget:
                    NormalMove();
                    break;
                case Behaviour.Stealth:
                    NormalMove();
                    break;
                case Behaviour.AttackImmediately:
                    if (seenPlayer) { 
                        combatTarget = GlobalRefs.Instance.player.transform;
                        targetTransform = combatTarget;
                        ToggleCombat(true);
                        StartCoroutine(nameof(GetNewTargetPos));
                        StartCoroutine(target.InitTargetValues(combatTarget, combatTarget.GetComponent<Rigidbody2D>()));
                    }
                    NormalMove();
                    break;
                default:
                    break;
            }
        }
        //AIStep();
        
        //Low health check
        CheckIfLowHealth();

        //Get target when hit
        CheckForNewCombatTargetWhenDamageTaken();
        
        lastFrameHealth = healthModule.currentHealth;
        if (inCombat)
            HandleSpecialAttack();
    }
    void HandleSpecialAttack()
    {
        if (ship.specialAttackCD != 0 && curCD > ship.specialAttackCD * 60)
        {
            StartCoroutine(InitSpecialAttack());
            curCD = 0;
        }
        curCD++;
    }
    void ToggleCombat(bool a)
    {
        if (a)
        {
            Debug.Log("Toggling combat on");
            rb.drag = ship.combatDrag;
            rb.angularDrag = ship.combatAngularDrag;
        }
        else
        {
            Debug.Log("Toggling combat off");
            rb.drag = 1;
            rb.angularDrag = 1;
            combatTarget = null;
            targetTransform = null;
            StartCoroutine(target.ClearTarget());
        }
        inCombat = a;
    }
    void NormalMove()
    {
        Thruster();
        if (rotate) Rotate();
        Strafe();
        if (ship.canRepair) Repair();
    }
    IEnumerator InitSpecialAttack()
    {
        if(ship.stopCoreWhenSpecial && target.target) {
            idle = true;
            targetPos = (Vector2)target.target.position
                        + (target.targetRB.velocity
                        * (Vector2.Distance(transform.position, target.target.position)
                        / ship.specialAttackSpeed));
            yield return new WaitForSeconds(0.2f);
            targetPos = (Vector2)target.target.position
                            + (target.targetRB.velocity
                            * (Vector2.Distance(transform.position, target.target.position)
                            / ship.specialAttackSpeed));
            yield return new WaitForSeconds(0.2f);
            targetPos = (Vector2)target.target.position
                            + (target.targetRB.velocity
                            * (Vector2.Distance(transform.position, target.target.position)
                            / ship.specialAttackSpeed));
            yield return new WaitForSeconds(0.2f);
            targetPos = (Vector2)target.target.position
                            + (target.targetRB.velocity
                            * (Vector2.Distance(transform.position, target.target.position)
                            / ship.specialAttackSpeed));
            yield return new WaitForSeconds(0.1f);
        }
        curCD = 0;
        
        FireSpecialAttack();
        curCD = 0;
        yield return new WaitForSeconds(0.2f);
        if (ship.stopCoreWhenSpecial)
        {
            idle = false;
            StartCoroutine(GetNewTargetPos());
        }
        curCD = 0;
        yield return null;
    }
    public void FireSpecialAttack()
    {
        switch (ship.specialAttack)
        {
            case SpecialAttack.None:
                break;
            case SpecialAttack.ElectricBall:
                GameObject bullet = Instantiate(EnemyManager.Instance.ElectricBallPrefab, transform.position, new Quaternion(), aiBullets.transform);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
                bullet.GetComponent<AIBullet>().damage = ship.specialAttackDamage;
                bullet.GetComponent<AIBullet>().damageTimer = ship.damageTickTime;
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * ship.specialAttackSpeed, ForceMode2D.Impulse);
                break;
            case SpecialAttack.HomingMissiles:
                break;
            case SpecialAttack.VoidNova:
                break;
            case SpecialAttack.SelfDestruct:
                break;
            default:
                break;
        }
    }
    float CheckDistanceAndSetModifier()
    {
        float distCheckToPlayer = Extension.Distance(transform.position, GlobalRefs.Instance.player.transform.position);
        if (lowHealth && distCheckToPlayer > ship.targetRadius * ship.targetRadius)
        {
            repairing = true;
            return ship.repairMod;
        }
        else if (lowHealth && distCheckToPlayer < ship.targetRadius * ship.targetRadius)
        {
            if (healthModule.currentHealth <= healthModule.startHealth / 5)
            {
                repairing = false;
                return ship.fleeMod;
            }
            else
            {
                repairing = false;
                lowHealth = false;
                return ship.fleeMod;
            }
        }
        else if (inCombat) return ship.combatMod;
        else return ship.exploreMod;
    }

    void CheckIfLowHealth()
    {
        if (!lowHealth)
        {
            if (healthModule.currentHealth <= healthModule.startHealth / 5)
            {
                lowHealth = true;
                StartCoroutine(nameof(GetNewTargetPos));
            }
        }
    }

    void CheckForNewCombatTargetWhenDamageTaken()
    {
        if (healthModule.damageTaken && healthModule.damageTakenFromWhat)
        {
            if (!lowHealth)
            {
                if (Extension.Distance(transform.position, healthModule.damageTakenFromWhat.transform.position) < ship.reactRange * ship.reactRange)
                {
                    combatTarget = healthModule.damageTakenFromWhat.transform;
                    targetTransform = combatTarget; 
                    ToggleCombat(true);
                    StartCoroutine(nameof(GetNewTargetPos));
                    StartCoroutine(target.InitTargetValues(combatTarget, combatTarget.GetComponent<Rigidbody2D>()));
                    healthModule.damageTaken = false;
                    healthModule.damageTakenFromWhat = null;

                    return;
                }
                Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, ship.reactRange, targetMask);
                foreach (Collider2D t in potentialTargets)
                {
                    //If the found target is this ship, return early
                    if (t == col) { return; }
                    if (Random.value > ship.changeTargetChance) { return; }
                    combatTarget = t.transform;
                    targetTransform = combatTarget;
                    ToggleCombat(true);
                    StartCoroutine(nameof(GetNewTargetPos));
                    StartCoroutine(target.InitTargetValues(combatTarget, combatTarget.GetComponent<Rigidbody2D>()));
                    healthModule.damageTaken = false;
                    healthModule.damageTakenFromWhat = null;
                }
            }
        }
        healthModule.damageTaken = false;
        healthModule.damageTakenFromWhat = null;
    }

    void TryStopCombat()
    {
        if (inCombat && (!target.target || !target.targetRB))
        {
            ToggleCombat(false);
            StartCoroutine(nameof(GetNewTargetPos));
            return;
        }
        if (inCombat)
        {
            if (lowHealth || combatTarget == null)
            {
                ToggleCombat(false);
                StartCoroutine(nameof(GetNewTargetPos));
            }
        }
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
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, ship.viewRange, avoidMask);
            if (objects != null)
            {
                _distance = ship.viewRange;
                foreach (Collider2D avoid in objects)
                {
                    if (avoid != col)
                    {
                        if (!seenPlayer && avoid.gameObject.layer == 6) //Layer 6 is Player
                        { seenPlayer = true; targetTransform = avoid.transform; }
                        float distCheckProx = Extension.Distance((Vector2)transform.position, avoid.ClosestPoint(transform.position));
                        if (distCheckProx < _distance)
                        {
                            _distance = Vector2.Distance((Vector2)transform.position, avoid.ClosestPoint(transform.position));
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
            //Check if idle
            if (!inCombat && !lowHealth && Random.Range(1, 3) == 1)
                idle = true;
            else idle = false;
            StartCoroutine(nameof(GetNewTargetPos));
            yield return new WaitForSeconds(Random.Range(ship.newMoveTargetTime.x / currentModifier, ship.newMoveTargetTime.y / currentModifier));
        }
    }
    IEnumerator GetNewTargetPos()
    {
        if (lowHealth)
        {
            switch (ship.lowHealthBehaviour)
            {
                case LowHealthBehaviour.None:
                    break;
                case LowHealthBehaviour.Flee:
                    FindPosFarAway();
                    break;
                case LowHealthBehaviour.Jump:
                    FindPosFarAway();
                    break;
                case LowHealthBehaviour.DropLootAndJump:
                    FindPosFarAway();
                    break;
                case LowHealthBehaviour.Surrender:
                    break;
                case LowHealthBehaviour.SurrenderButBargain:
                    break;
                case LowHealthBehaviour.SurrenderButTrick:
                    break;
                case LowHealthBehaviour.Enrage:
                    FindPosNearTarget();
                    break;
                case LowHealthBehaviour.SelfDestruct:
                    FindPosNearTarget();
                    break;
                default:
                    break;
            }
            
        }
        else if (inCombat)
        {
            switch (ship.combatBehaviour)
            {
                case CombatBehaviour.None:
                    break;
                case CombatBehaviour.StandGroundAndAttack:
                    FindPosOnTarget();
                    break;
                case CombatBehaviour.RotateTowardsAndAttack:
                    FindPosOnTarget();
                    break;
                case CombatBehaviour.ChaseAndAttack:
                    FindPosNearTarget();
                    break;
                case CombatBehaviour.RunAndAttack:
                    FindPosFarAway();
                    break;
                case CombatBehaviour.SelfDestruct:
                    FindPosOnTarget();
                    break;
                case CombatBehaviour.Crash:
                    FindPosOnTarget();
                    break;
                case CombatBehaviour.JumpImmediately:
                    FindPosFarAway();
                    break;
                default:
                    break;
            }
            
        }
        else if (targetTransform && target.target)
        {
            switch (ship.behaviour)
            {
                case Behaviour.None:
                    break;
                case Behaviour.NotDefined:
                    FindPosNearSpawn();
                    break;
                case Behaviour.TargetResources:
                    FindResource();
                    break;
                case Behaviour.StalkTarget:
                    FindPosNearTarget();
                    break;
                case Behaviour.JumpOnSight:
                    FindPosFarAway();
                    break;
                case Behaviour.RepairTarget:
                    FindPosNearTarget();
                    break;
                case Behaviour.Stealth:
                    FindPosNearTarget();
                    break;
                case Behaviour.AttackImmediately:
                    FindPosNearTarget();
                    break;
                default:
                    break;
            }
        }
        else FindPosNearSpawn();
        yield return null;
    }
    void FindPosFarAway()
    {
        targetPos = 10 * ship.targetRadius * (Vector2)transform.up;
    }
    void FindPosNearTarget()
    {
        targetPos = (Vector2)targetTransform.position + Random.insideUnitCircle * ship.targetRadius;
    }
    void FindPosOnTarget()
    {
        targetPos = (Vector2)targetTransform.position;
    }
    void FindPosNearSpawn()
    {
        targetPos = spawnPoint + Random.insideUnitCircle * ship.targetRadius * 2;
    }
    void FindResource()
    {
        Collider2D[] pickup = Physics2D.OverlapCircleAll(transform.position, ship.viewRange, pickupMask);
        if (pickup != null)
        {
            targetPos = pickup[0].transform.position;
        }
    }
    void Thruster()
    {
        var tEmis = thrustersPS.emission;
        if (idle && !inCombat && !lowHealth)
        {
            if (tEmis.enabled)
                tEmis.enabled = false;
        }
        else
        {
            if (!tEmis.enabled)
                tEmis.enabled = true;
            rb.AddForce(Mathf.Lerp(0.2f, 1f, (_distance / ship.viewRange)) * currentModifier * ship.speed * transform.up, ForceMode2D.Force);
        }
    }
    void Rotate()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(-ship.rotSpeed * currentModifier * rotateAmount, ForceMode2D.Force);
    }
    void Strafe()
    {
        ps.transform.rotation = Quaternion.RotateTowards(ps.transform.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, -dir.normalized)), 10);
        var pEmission = ps.emission;
        if (dir != new Vector2(0, 0))
        {
            pEmission.rateOverTime = (ship.viewRange - _distance) * 20f;
        }
        else if (!ps.isPaused)
        {
            pEmission.rateOverTime = 0;
        }
        rb.AddForce(((ship.viewRange + 1f - _distance) / 2) * currentModifier * ship.strafeSpeed * dir.normalized, ForceMode2D.Force);
    }
    void Repair()
    {
        if (repairing)
        {
            if (healthModule.currentHealth < healthModule.startHealth)
            {
                healthModule.currentHealth += ship.repairSpeed;
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
    void Jump()
    {
        jumping = true;
        var tEmis = thrustersPS.emission;
        if (jumpTimer > ship.jumpTime * 60)
        {
            if (!tEmis.enabled)
                tEmis.enabled = true;
            if (!GetComponent<Collider2D>().isTrigger)
                GetComponent<Collider2D>().isTrigger = true;
            rb.AddForce(currentModifier * ship.jumpSpeed * ship.speed * transform.up, ForceMode2D.Force);
        }
        jumpTimer++;
        if (jumpTimer > ship.jumpTime * 60 * 2)
            Destroy(gameObject);
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
