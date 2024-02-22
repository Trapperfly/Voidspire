using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class GunFire : MonoBehaviour
{
    Rigidbody2D pRB;
    GunStats stat;
    Transform bulletHolder;
    EquipmentController gc;
    public Weapon w;
    [SerializeField] AdjustToTarget target;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GunMaster gunMaster;
    [SerializeField] GunPoint gunPoint;

    [SerializeField] LayerMask rayHitMask;
    bool beamActive;

    float fireRateA;
    float spreadA;
    bool inBurst = false;
    bool chargeAvailable = true;
    float charge;
    int extraShot = 0;
    int gunTimer = 0;
    float fireRateScalar = 0;
    float spreadScalar;

    private void Awake()
    {
        pRB = GlobalRefs.Instance.player.GetComponent<Rigidbody2D>();
        EquipmentController.Instance.onGunLoadComplete += CustomStart;
        stat = GetComponent<GunStats>();
        gunPoint = GetComponent<GunPoint>();
    }
    private void Start()
    {
        //gc = GunController.Instance;
        //Debug.Log(gc);
        //w = gc.weapons[stat.gunNumber].item as Weapon;
        //bulletHolder = gc.bc[stat.gunNumber].transform;
        //fireRateA = w.fireRate;
        //spreadA = w.spread;
    }

    private void CustomStart()
    {
        //Debug.Log("CustomStartActivated");
        gc = EquipmentController.Instance;
        //Debug.Log(gc);
        SetNewWeapon();
        bulletHolder = gc.bc[stat.gunNumber].transform;
        fireRateA = w.fireRate;
        spreadA = w.spread;
        EquipmentController.Instance.onGunLoadComplete -= CustomStart;
        EquipmentController.Instance.onGunLoadComplete += SetNewWeapon;
    }

    public void SetNewWeapon()
    {
        w = gc.weaponSlots[stat.gunNumber].item as Weapon;
        SpriteRenderer wSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (w) { wSprite.sprite = w.icon; wSprite.enabled = true; gunPoint.rotSpeed = w.rotationSpeed; }
        else { wSprite.enabled = false; wSprite.sprite = null; }
        stat.active = w != null;
    }
    private void FixedUpdate()
    {
        if (GlobalRefs.Instance.playerIsDead) return;
        if (stat.aimed && stat.active && Input.GetKey(KeyCode.Mouse0) && gunTimer >= Mathf.Clamp(60 / fireRateA, 1, 600))
        {
            if (w.fireRateChange != 0 && w.fireRateChangeTimer != 0)
            {
                fireRateScalar += (1 / fireRateA) / w.fireRateChangeTimer;
            }
            if (w.spreadChange != 0 && w.spreadChangeTimer != 0)
            {
                spreadScalar += (1 / fireRateA) / w.spreadChangeTimer;
            }
            FindExtraShotChance(fireRateA);
            if (w.chargeUp == 0 || charge > w.chargeUp * 60)
            {
                if (w.burst != 0 && !inBurst)
                {
                    StartCoroutine(Burst(w.burst, w.burstDelay));
                }
                else if (!inBurst)
                {
                    for (int i = w.amount + extraShot; i > 0; i--)
                    {
                        Shoot();
                        charge = 0;
                        chargeAvailable = false;
                    }
                    extraShot = 0;
                }
            }
            else if (w.chargeUp != 0 && chargeAvailable == true)
            {
                charge++;
            }
        }
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            if (charge >= 2)
            {
                charge -= 2;
            }
            chargeAvailable = true;
            if (fireRateScalar > 0)
            {
                fireRateScalar -= (1f / (60f)) / w.fireRateChangeTimer * 2;
            }
            if (spreadScalar > 0)
            {
                spreadScalar -= (1f / (60f)) / w.spreadChangeTimer * 2;
            }
        }
        if (stat.active)
        {
            fireRateA = Mathf.Lerp(w.fireRate, w.fireRate + w.fireRateChange, fireRateScalar);
            if (fireRateScalar < 0 || fireRateScalar > 1)
                fireRateScalar = Mathf.Clamp(fireRateScalar, 0, 1);
            if (fireRateA != w.fireRate && fireRateScalar == 0)
                fireRateA = w.fireRate;

            spreadA = Mathf.Lerp(w.spread, w.spread + w.spreadChange, spreadScalar);
            if (spreadScalar < 0 || spreadScalar > 1)
                spreadScalar = Mathf.Clamp(spreadScalar, 0, 1);
            if (spreadA != w.spread && spreadScalar == 0)
                spreadA = w.spread;
            gunTimer++;
        }
        else { spreadA = 0; fireRateA = 0; gunTimer = 0; }
    }
    IEnumerator Burst(int times, float delay)
    {
        inBurst = true;
        for (int b = times; b > 0; b--)
        {
            for (int i = w.amount + extraShot; i > 0; i--)
            {
                Shoot();
                charge = 0;
                chargeAvailable = false;
            }
            yield return new WaitForSeconds(delay);
        }
        yield return null;
        inBurst = false;
        extraShot = 0;
    }

    void Shoot()
    {
        switch (w.weaponType)
        {
            case WeaponType.Bullet:
                StartCoroutine(ShootBullet());
                break;
            case WeaponType.Laser:
                StartCoroutine(ShootLaser());
                break;
            case WeaponType.Wave:
                StartCoroutine(ShootWave());
                break;
            case WeaponType.Rocket:
                StartCoroutine(ShootRocket());
                break;
            case WeaponType.Needle:
                StartCoroutine(ShootNeedle());
                break;
            case WeaponType.Railgun:
                StartCoroutine(ShootRailgun());
                break;
            case WeaponType.Mine:
                StartCoroutine(ShootMine());
                break;
            case WeaponType.Hammer:
                StartCoroutine(ShootHammer());
                break;
            case WeaponType.Cluster:
                StartCoroutine(ShootCluster());
                break;
            case WeaponType.Arrow:
                StartCoroutine(ShootArrow());
                break;
            case WeaponType.Mirage:
                break;
            case WeaponType.Grand:
                StartCoroutine(ShootGrand());
                break;
            case WeaponType.Void:
                break;
            case WeaponType.Beam:
                if (!beamActive) StartCoroutine(ShootBeam());
                break;
            case WeaponType.Blade:
                break;
            default:
                break;
        }
    }
    public IEnumerator ShootBullet()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.bulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }

    public IEnumerator ShootBeam()
    {
        beamActive = true;
        LineRenderer beam = Instantiate(gc.beamPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation, bulletSpawnPoint).GetComponent<LineRenderer>();
        ParticleSystem beamHitPs = Instantiate(gc.beamPsPrefab).GetComponent<ParticleSystem>();
        beam.material.SetColor("_TrailColor", w.effectColor);
        var trails = beamHitPs.trails;
        trails.colorOverTrail = w.effectColor;
        var emis = beamHitPs.emission;
        gunMaster.hasFired = true;
        gunTimer = 0;
        while (Input.GetMouseButton(0) && stat.active)
        {
            //Calculate baser and hit
            RaycastHit2D[] hit = Physics2D.RaycastAll(bulletSpawnPoint.position, bulletSpawnPoint.up, w.longevity * w.speed / 5, rayHitMask);
            int actualHits = 0;
            Damagable dm = null;
            for (int i = 0; i <= w.pierce; i++)
            {
                if (InBounds(i, hit)) { actualHits++; }
                if (InBounds(i, hit) && hit[i].collider.GetComponent<Damagable>())
                    dm = hit[i].collider.GetComponent<Damagable>();
                if (InBounds(i, hit) && dm) 
                {
                    float damage = w.damage * Time.deltaTime * fireRateA;
                    if ( Random.value > 0.9f )
                    {
                        dm.TakeDamage(damage * 10, hit[i].point, GlobalRefs.Instance.player);
                    }
                }
                    
            }

            

            //Viusal effects
            if (actualHits <= w.pierce)
            {
                beam.SetPosition(1, new Vector2(0, 1) * w.longevity * w.speed / 5);
                emis.enabled = false;
            }
            else
            {
                beam.SetPosition(1, bulletSpawnPoint.InverseTransformPoint(hit[actualHits-1].point));
                beamHitPs.transform.position = hit[actualHits - 1].point;
                beamHitPs.transform.LookAt(bulletSpawnPoint);
                emis.enabled = true;
            }

            yield return new WaitForEndOfFrame();
        }
        Destroy(beam.gameObject);
        emis.enabled = false;
        Destroy(beamHitPs.gameObject, 0.5f);
        beamActive = false;
    }

    IEnumerator ShootRailgun()
    {
        bulletSpawnPoint.rotation = Spread(bulletSpawnPoint.rotation);
        LineRenderer hitscan = 
            Instantiate
            (
                gc.railgunPrefab, 
                bulletSpawnPoint.TransformPoint(bulletSpawnPoint.transform.position), 
                new Quaternion(0,0,0,0), 
                bulletHolder
            )
            .GetComponent<LineRenderer>();
        ParticleSystem hitscanLinePs =
            Instantiate
            (
                gc.railgunLinePsPrefab,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation,
                null
            )
            .GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = hitscanLinePs.shape;
        ParticleSystem.Burst burst = hitscanLinePs.emission.GetBurst(0);
        RaycastHit2D[] hit = Physics2D.RaycastAll(bulletSpawnPoint.position, bulletSpawnPoint.up, w.longevity * w.speed / 5, rayHitMask);
        Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.up);
        Damagable dm = null;
        int actualHits = 0;
        for (int i = 0; i <= w.pierce; i++)
        {
            if (InBounds(i, hit)) { actualHits++; }
            if (InBounds(i, hit) && hit[i].collider.GetComponent<Damagable>())
                dm = hit[i].collider.GetComponent<Damagable>();
            if (InBounds(i, hit) && dm)
                dm.TakeDamage(w.damage, hit[i].point, GlobalRefs.Instance.player);
        }
        Debug.Log(actualHits);


        //Viusal effects
        Debug.Log(w.pierce);
        if (actualHits <= w.pierce)
        {
            hitscan.SetPosition(0, bulletSpawnPoint.transform.position);
            hitscan.SetPosition(1, bulletSpawnPoint.transform.position + (w.longevity * w.speed * bulletSpawnPoint.up / 5));
        }
        else
        {
            ParticleSystem hitscanHitPs = Instantiate(gc.railgunPsPrefab).GetComponent<ParticleSystem>();
            hitscan.SetPosition(0, bulletSpawnPoint.transform.position);
            hitscan.SetPosition(1, hit[actualHits-1].point);
            hitscanHitPs.transform.position = hit[actualHits-1].point;
            hitscanHitPs.transform.LookAt(bulletSpawnPoint);
            hitscanHitPs.Play();
            Destroy(hitscanHitPs.gameObject, 0.5f);
        }
        Destroy(hitscan.gameObject);

        hitscanLinePs.transform.position = hitscan.GetPosition(1) + (hitscan.GetPosition(0) - hitscan.GetPosition(1)) / 2;
        float length = Vector2.Distance(hitscan.GetPosition(0), hitscan.GetPosition(1));
        shape.radius = length / 2;
        ParticleSystem.Burst newBurst = new(0, length * 100);
        hitscanLinePs.emission.SetBurst(0, newBurst);
        hitscanLinePs.Play();
        Destroy(hitscanLinePs.gameObject, 1f);
        yield return null;

        gunTimer = 0;
        gunMaster.hasFired = true;
        bulletSpawnPoint.rotation = transform.rotation;
        yield return null;
    }

    public IEnumerator ShootLaser()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.laserPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }

    public IEnumerator ShootWave()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.wavePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }

    public IEnumerator ShootRocket()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.rocketPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    public IEnumerator ShootNeedle()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.needlePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    public IEnumerator ShootMine()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.minePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    public IEnumerator ShootHammer()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.hammerPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    public IEnumerator ShootCluster()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.clusterPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    public IEnumerator ShootArrow()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.arrowPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    public IEnumerator ShootGrand()
    {
        //Debug.Log("Trying to shoot bullet");
        GameObject bullet = Instantiate(gc.grandPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc[stat.gunNumber];
        bullet.transform.localScale *= w.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = pRB.velocity + (Vector2)(bullet.transform.up * Speed(w.speed));
        gunTimer = 0;
        gunMaster.hasFired = true;
        bullet.GetComponent<Bullet>().bulletSender = GlobalRefs.Instance.player;
        yield return null;
    }
    private bool InBounds(int index, RaycastHit2D[] array)
    {
        return (index >= 0) && (index < array.Length);
    }
    private void Update()
    {
        Debug.DrawRay(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.up);
    }
    float Speed(float baseSpeed)
    {
        float spreadSpeed = baseSpeed;
        if (w.amount > 1)
        {
            if (Random.value >= .5f)
                spreadSpeed *= 1 + CurveWeightedRandom(stat.speedCurve);
            else
                spreadSpeed *= 1 - CurveWeightedRandom(stat.speedCurve);
        }
        return spreadSpeed;
    }
    Quaternion Spread(Quaternion baseRotation)
    {
        spreadA *= CurveWeightedRandom(stat.spreadCurve);
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-spreadA, spreadA));
        return spreadValue;
    }
    void FindExtraShotChance(float _fr)
    {
        float chance = (_fr - 60) / 60;
        float chanceModulus = chance % 1;
        extraShot = Mathf.Clamp(Mathf.FloorToInt(chance), 0, 100);
        if (Random.value <= chanceModulus)
        {
            extraShot += 1;
        }
    }
    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
}
