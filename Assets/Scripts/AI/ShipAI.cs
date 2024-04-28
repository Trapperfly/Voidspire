using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;



public class ShipAI : MonoBehaviour
{
    public bool isBoss;
    bool playerIsDead;
    public int level = 0;
    public Ship ship;
    float currentModifier = 1f;
    bool repairing;
    bool firingSpecial;
    bool rotate = true;
    public bool jumping;
    bool jumped;
    public Transform aiBullets;

    public AIEquipGun[] guns;

    public int curCD = 0;

    Transform combatTarget;
    public List<Vector2> dirs = new List<Vector2>();

    public ActiveTarget target;
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
    bool enrageActive = false;

    [HideInInspector] public bool doubleAttackSpeed;
    [HideInInspector] public bool doubleDamage;
    [HideInInspector] public bool enabledSpecialAttack;
    [HideInInspector] public bool unlockedGuns;

    float speedScaling;
    Vector2 combatTScaling;
    float healthScaling;

    //public Transform hud;
    //public GameObject healthBarAndLevel;
    //public GameObject bossHealthBarAndLevel;
    //Image healthBar;
    //TMP_Text levelText;

    List<ParticleSystem> toDestroy = new();

    public Com contact;
    public Com lowHealthContact;

    StudioEventEmitter fireMissileEmitter;
    StudioEventEmitter fireEleBallEmitter;
    StudioEventEmitter fireBeamEmitter;

    StudioEventEmitter enemyChargeFTLEmitter;
    StudioEventEmitter enemyEnterFTLEmitter;

    private void Start()
    {
        contact = ship.possibleComs.Length > 0 ? ship.possibleComs[Random.Range(0, ship.possibleComs.Length - 1)] : ship.baseCom;
        lowHealthContact = ship.possibleLowHealthComs.Length > 0 ? ship.possibleLowHealthComs[Random.Range(0, ship.possibleLowHealthComs.Length - 1)] : ship.baseCom;
        if (level == 0)
            level += (GlobalRefs.Instance.currentSector - 1) * 10;
        //Init();
        var tEmis = thrustersPS.emission;
        tEmis.enabled = false;
        aiBullets = EnemyManager.Instance.bh;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        StartCoroutine(nameof(CheckProximity));

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        targetPos = GlobalRefs.Instance.player.transform.position;
        spawnPoint = transform.position;
        StartCoroutine(nameof(GetNewTargetPosOverTime));
        dir = transform.up;

        rb.mass = ship.mass;

        float tempSpeed = ship.speed;
        for (int i = 0; i < level; i++)
        {
            tempSpeed *= 1 + Difficulty.dif.AISpeedIncreasePerLevel;
        }
        speedScaling = tempSpeed;
        float tempCTx = ship.newMoveTargetTime.x;
        float tempCTy = ship.newMoveTargetTime.y;
        for (int i = 0; i < level; i++)
        {
            tempCTx *= 1 + Difficulty.dif.AICombatTimeIncreasePerLevel;
            tempCTy *= 1 + Difficulty.dif.AICombatTimeIncreasePerLevel;
        }
        combatTScaling = new Vector2(tempCTx, tempCTy);
        float tempMaxHealth = ship.maxHealth;
        for (int i = 0; i < level; i++)
        {
            tempMaxHealth *= 1 + Difficulty.dif.AIHealthIncreasePerLevel;
        } 
        healthScaling = tempMaxHealth;
        Debug.Log(healthScaling);

        healthModule = GetComponent<Damagable>();
        healthModule.startHealth = healthScaling;
        healthModule.currentHealth = healthScaling;
        lastFrameHealth = healthModule.currentHealth;
        healthModule.healthPercent = healthModule.currentHealth / healthModule.startHealth;

        if(ship.startSpecialImmediately) { curCD = Mathf.RoundToInt(ship.specialAttackCD * 60); }
    }

    //void Init()
    //{
    //    if (isBoss)
    //    {
    //        gameObject.name = ship.aiName;
    //        hud = GameObject.FindGameObjectWithTag("Hud").transform;
    //        GameObject b = Instantiate(bossHealthBarAndLevel, hud);
    //        healthBar = b.transform.GetChild(2).GetChild(2).GetComponent<Image>();
    //        b.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = gameObject.name;
    //        levelText = b.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
    //        levelText.text = level.ToString();
    //        b.GetComponent<EnemyHealthBar>().target = transform;
    //        return;
    //    }
    //    gameObject.name = ship.aiName;
    //    hud = GameObject.FindGameObjectWithTag("Hud").transform;
    //    GameObject h = Instantiate(healthBarAndLevel, transform.position, new Quaternion(), hud);
    //    healthBar = h.transform.GetChild(1).GetChild(2).GetComponent<Image>();
    //    levelText = h.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
    //    levelText.text = level.ToString();
    //    h.GetComponent<EnemyHealthBar>().target = transform;
    //}
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
                    //Debug.Log("IIIIIIIIIIIIIIIIIIIIIIIIIIII");
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
                        //Debug.LogWarning("Something");
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
        if (lowHealth && isEnraged) { ActivateEnrage(); }

        //Get target when hit
        CheckForNewCombatTargetWhenDamageTaken();
        
        lastFrameHealth = healthModule.currentHealth;
        if (inCombat)
            HandleSpecialAttack();

        if (target.target == GlobalRefs.Instance.player.transform && GlobalRefs.Instance.playerIsDead) { playerIsDead = true; target.target = null; seenPlayer = false; }
            
    }


    //Draw debug rays for vectors in CheckProximity
    private void Update()
    {
        //healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthModule.healthPercent, 0.1f);
        foreach (Vector2 line in dirs)
        {
            Debug.DrawRay(transform.position, line.normalized);
        }
    }
    void ActivateEnrage()
    {
        if (enrageActive) return;
        enrageActive = true;
        switch (ship.enrageBehaviour)
        {
            case EnrageBehaviour.DoubleAttackSpeed:
                doubleAttackSpeed = true;
                break;
            case EnrageBehaviour.DoubleDamage:
                doubleDamage = true;
                break;
            case EnrageBehaviour.EnableSpecialAttack:
                enabledSpecialAttack = true;
                break;
            case EnrageBehaviour.SpawnAllies:
                break;
            case EnrageBehaviour.UnlockLockedGuns:
                unlockedGuns = true;
                break;
            default:
                break;
        }
    }
    void HandleSpecialAttack()
    {
        if (enabledSpecialAttack && ship.specialAttackCD != 0 && curCD > ship.specialAttackCD * 60)
        {
            StartCoroutine(InitSpecialAttack(ship.enrageSpecialAttack));
            curCD = 0;
            curCD++;
            return;
        }
        if (ship.specialAttack != SpecialAttack.None && ship.specialAttackCD != 0 && curCD > ship.specialAttackCD * 60)
        {
            StartCoroutine(InitSpecialAttack(ship.specialAttack));
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
    IEnumerator InitSpecialAttack(SpecialAttack special)
    {
        if(ship.aimTheSpecial && target.target) {
            if (ship.stopCoreWhenSpecial) idle = true;
            for (int i = 0; i < 4; i++)
            {
                if (target.target) {
                    if (ship.predictWhenAiming)
                        targetPos = (Vector2)target.target.position
                            + (target.targetRB.velocity
                            * (Vector2.Distance(transform.position, target.target.position)
                            / ship.specialAttackSpeed));
                    else targetPos = (Vector2)target.target.position;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        curCD = 0;
        
        FireSpecialAttack(special);
        if (ship.aimTheSpecial && target.target)
        {
            if (ship.stopCoreWhenSpecial) idle = true;
            for (int i = 0; i < ship.stopCoreForSeconds * 10; i++)
            {
                if (target.target)
                {
                    if (ship.predictWhenAiming)
                        targetPos = (Vector2)target.target.position
                            + (target.targetRB.velocity
                            * (Vector2.Distance(transform.position, target.target.position)
                            / ship.specialAttackSpeed));
                    else targetPos = (Vector2)target.target.position;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (ship.stopCoreWhenSpecial || ship.stopCoreWhenSpecialLong)
        {
            idle = false;
            StartCoroutine(GetNewTargetPos());
        }
        curCD = 0;
        yield return null;
    }
    public void FireSpecialAttack(SpecialAttack special)
    {
        Transform bh;
        if (ship.specialIsHoming) { bh = EnemyManager.Instance.hbh; }
        else bh = EnemyManager.Instance.bh;
        switch (special)
        {
            case SpecialAttack.None:
                break;
            case SpecialAttack.ElectricBall:
                StartCoroutine(FireElectricBalls(bh));
                break;
            case SpecialAttack.HomingMissiles:
                StartCoroutine(FireMissiles(bh));
                break;
            case SpecialAttack.VoidNova:
                break;
            case SpecialAttack.SelfDestruct:
                break;
            case SpecialAttack.DestroyerBeam:
                StartCoroutine(ShootBeam());
                break;
            default:
                break;
        }
    }
    IEnumerator FireElectricBalls(Transform parent)
    {
        for (int i = 0; i < ship.specialAmount; i++)
        {
            fireEleBallEmitter = AudioManager.Instance.InitEmitter(FMODEvents.Instance.electricBallFire, gameObject);
            fireEleBallEmitter.Play();
            AIBullet bullet = Instantiate
                (EnemyManager.Instance.ElectricBallPrefab,
                transform.position, Spread(transform.rotation,
                ship.specialAttackSpread),
                parent).GetComponent<AIBullet>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
            bullet.damageTimer = ship.damageTickTime;
            bullet.homing = true;
            bullet.homingStrength = ship.specialHomingStrength;
            bullet.target = target.target;
            bullet.damage = ship.specialAttackDamage;
            bullet.speed = ship.specialAttackSpeed;
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * ship.specialAttackSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    IEnumerator FireMissiles(Transform parent)
    {
        for (int i = 0; i < ship.specialAmount; i++)
        {
            AIBullet bullet = Instantiate
                (EnemyManager.Instance.HomingMissilesPrefab, 
                transform.position, Spread(transform.rotation, 
                ship.specialAttackSpread), 
                parent).GetComponent<AIBullet>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
            bullet.homing = true;
            bullet.homingStrength = ship.specialHomingStrength;
            bullet.target = target.target;
            bullet.damage = ship.specialAttackDamage;
            bullet.speed = ship.specialAttackSpeed;
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * ship.specialAttackSpeed, ForceMode2D.Impulse);
            fireMissileEmitter = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireMissile, gameObject);
            fireMissileEmitter.Play();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public IEnumerator ShootBeam()
    {
        bool active = true;
        float timer = 0;
        LineRenderer beam = Instantiate(EnemyManager.Instance.beamPrefab, transform.position, transform.rotation, transform).GetComponent<LineRenderer>();
        ParticleSystem beamHitPs = Instantiate(EnemyManager.Instance.beamTipPrefab).GetComponent<ParticleSystem>();
        toDestroy.Add(beamHitPs);
        beam.material.SetColor("_TrailColor", EnemyManager.Instance.beamColor);
        var trails = beamHitPs.trails;
        trails.colorOverTrail = EnemyManager.Instance.beamColor;
        var emis = beamHitPs.emission;
        fireBeamEmitter = AudioManager.Instance.InitEmitter(FMODEvents.Instance.beam, gameObject);
        fireBeamEmitter.Play();
        while (active)
        {
            //Calculate baser and hit
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, ship.specialAttackSpeed, ship.specialHitMask);
            if (hit.collider)
            {
                hit.collider.TryGetComponent(out Damagable dm);
                hit.collider.TryGetComponent(out PlayerHealth ph);
                hit.collider.TryGetComponent(out PassiveShieldCollider sh);
                float damage = ship.specialAttackDamage * Time.deltaTime;
                if (Random.value > 0.9f)
                {
                    if (dm) dm.TakeDamage(damage * 10, hit.point, gameObject);
                    if (ph) ph.TakeDamage(damage * 10);
                    if (sh) sh.TakeDamage(damage * 10);
                }
                //Viusal effects
                if (dm || ph || sh)
                {
                    beam.SetPosition(1, transform.InverseTransformPoint(hit.point));
                    beamHitPs.transform.position = hit.point;
                    beamHitPs.transform.LookAt(transform);
                    emis.enabled = true;
                }
            }
            else
            {
                beam.SetPosition(1, new Vector2(0, 1) * ship.specialAttackSpeed);
                emis.enabled = false;
            }
            
            timer += Time.deltaTime;
            if(timer > ship.specialAmount)
                active = false;
            yield return new WaitForEndOfFrame();
        }
        fireBeamEmitter.Stop();
        Destroy(beam.gameObject);
        emis.enabled = false;
        Destroy(beamHitPs.gameObject, 0.5f);
    }
    Quaternion Spread(Quaternion baseRotation, float spread)
    {
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-spread, spread));
        return spreadValue;
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
            if (healthModule.currentHealth <= healthModule.startHealth * ship.lowHealthPercent)
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
            if (healthModule.currentHealth <= healthModule.startHealth * ship.lowHealthPercent)
            {
                Debug.Log(gameObject + " is now at low health");
                lowHealth = true;
                if(!enrageActive) { isEnraged = true; }
                StartCoroutine(nameof(GetNewTargetPos));
            }
        }
    }

    void CheckForNewCombatTargetWhenDamageTaken()
    {
        if (healthModule.damageTaken && healthModule.damageTakenFromWhat)
        {
            if (healthModule.damageTakenFromWhat.CompareTag(gameObject.tag)) return;
            if (combatTarget)
                if (Random.value > ship.changeTargetChance) { return; }
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
                    if (t.gameObject.CompareTag(gameObject.tag)) return;
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
    public void StartCombat(Transform whatToCombat)
    {
        combatTarget = whatToCombat;
        targetTransform = combatTarget;
        ToggleCombat(true);
        StartCoroutine(nameof(GetNewTargetPos));
        StartCoroutine(target.InitTargetValues(combatTarget, combatTarget.GetComponent<Rigidbody2D>()));
    }
    public void StopCombat()
    {
        ToggleCombat(false);
        StartCoroutine(nameof(GetNewTargetPos));
    }
    void TryStopCombat()
    {
        if (inCombat && (!target.target || !target.targetRB))
        {
            //Debug.LogWarning("Toggling off combat because of no target");
            ToggleCombat(false);
            StartCoroutine(nameof(GetNewTargetPos));
            return;
        }
        if (inCombat)
        {
            if (lowHealth && (
                ship.lowHealthBehaviour == LowHealthBehaviour.Flee
                || ship.lowHealthBehaviour == LowHealthBehaviour.Jump
                || ship.lowHealthBehaviour == LowHealthBehaviour.DropLootAndJump
                ))
            {
                //Debug.LogWarning("Toggling off combat because of this is wimpy");
                ToggleCombat(false);
                StartCoroutine(nameof(GetNewTargetPos));
                return;
            }
            if (combatTarget == null || !targetTransform)
            {
                //Debug.LogWarning("Toggling off combat because combat target is dead");
                ToggleCombat(false);
                StartCoroutine(nameof(GetNewTargetPos));
            }
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
                        if (!playerIsDead && !seenPlayer && avoid.gameObject.layer == 6) //Layer 6 is Player
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
            yield return new WaitForSeconds(
                Random.Range(ship.newMoveTargetTime.x / currentModifier * combatTScaling.x,
                ship.newMoveTargetTime.y / currentModifier * combatTScaling.y));
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
        if (targetTransform)
            targetPos = (Vector2)targetTransform.position + Random.insideUnitCircle * ship.targetRadius;
    }
    void FindPosOnTarget()
    {
        if (targetTransform)
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
            rb.AddForce
                (
                speedScaling
                * currentModifier 
                * Mathf.Lerp(0.2f, 1f, (_distance / ship.viewRange)) 
                * ship.mass 
                * ship.speed 
                * transform.up, ForceMode2D.Force);
        }
    }
    void Rotate()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.AddTorque(
            speedScaling
            * -ship.rotSpeed 
            * currentModifier 
            * rotateAmount 
            * ship.mass, ForceMode2D.Force);
    }
    void Strafe()
    {
        ps.transform.rotation = Quaternion.RotateTowards(ps.transform.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, -dir.normalized)), 10);
        var pEmission = ps.emission;
        if (dir != new Vector2(0, 0))
        {
            pEmission.rateOverTime = (ship.viewRange - _distance) * 2f;
        }
        else if (!ps.isPaused)
        {
            pEmission.rateOverTime = 0;
        }
        rb.AddForce(((ship.viewRange + 1f - _distance) / 2) * currentModifier * ship.strafeSpeed * ship.mass * dir.normalized, ForceMode2D.Force);
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
        if (!enemyChargeFTLEmitter.IsPlaying()) { 
            enemyChargeFTLEmitter = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyChargeFTL, gameObject); 
            enemyChargeFTLEmitter.Play(); }
        jumping = true;
        var tEmis = thrustersPS.emission;
        if (jumpTimer > ship.jumpTime * 60)
        {
            if (jumped) enemyChargeFTLEmitter.Stop();
            if (jumped) {
                enemyEnterFTLEmitter = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyEnterFTL, gameObject);
                enemyEnterFTLEmitter.Play();
            }
            
            jumped = false;
            if (!tEmis.enabled)
                tEmis.enabled = true;
            if (!GetComponent<Collider2D>().isTrigger)
                GetComponent<Collider2D>().isTrigger = true;
            rb.AddForce(currentModifier * ship.jumpSpeed * ship.speed * ship.mass * transform.up, ForceMode2D.Force);
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

    private void OnDestroy()
    {
        if (healthModule.currentHealth <= 0)
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.explosion, transform.position);
        foreach (var ps in toDestroy)
        {
            Destroy(ps);
        }
    }
}
