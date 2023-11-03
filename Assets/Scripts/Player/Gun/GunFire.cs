using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    GunStats stat;
    Transform bulletHolder;
    [SerializeField] GunController gc;
    [SerializeField] AdjustToTarget target;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GunMaster gunMaster;
    [SerializeField] GunPoint gunPoint;

    [SerializeField] LayerMask laserRayHitMask;
    bool laserActive;

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
        stat = GetComponent<GunStats>();
        gunPoint = GetComponent<GunPoint>();
    }
    private void Start()
    {
        bulletHolder = gc.bc.transform;
        fireRateA = stat.fireRate;
        spreadA = stat.spread;
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && gunTimer >= Mathf.Clamp(60 / fireRateA, 1, 600))
        {
            if (stat.fireRateChange != 0 && stat.fireRateChangeTimer != 0)
            {
                fireRateScalar += (1 / fireRateA) / stat.fireRateChangeTimer;
            }
            if (stat.spreadChange != 0 && stat.spreadChangeTimer != 0)
            {
                spreadScalar += (1 / fireRateA) / stat.spreadChangeTimer;
            }
            FindExtraShotChance(fireRateA);
            if (stat.chargeUp == 0 || charge > stat.chargeUp * 60)
            {
                if (stat.burst != 0 && !inBurst)
                {
                    StartCoroutine(Burst(stat.burst, stat.burstDelay));
                }
                else if (!inBurst)
                {
                    for (int i = stat.amount + extraShot; i > 0; i--)
                    {
                        Shoot();
                        charge = 0;
                        chargeAvailable = false;
                    }
                    extraShot = 0;
                }
            }
            else if (stat.chargeUp != 0 && chargeAvailable == true)
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
                fireRateScalar -= (1f / (60f)) / stat.fireRateChangeTimer * 2;
            }
            if (spreadScalar > 0)
            {
                spreadScalar -= (1f / (60f)) / stat.spreadChangeTimer * 2;
            }
        }
        fireRateA = Mathf.Lerp(stat.fireRate, stat.fireRate + stat.fireRateChange, fireRateScalar);
        if (fireRateScalar < 0 || fireRateScalar > 1)
            fireRateScalar = Mathf.Clamp(fireRateScalar, 0, 1);
        if (fireRateA != stat.fireRate && fireRateScalar == 0)
            fireRateA = stat.fireRate;

        spreadA = Mathf.Lerp(stat.spread, stat.spread + stat.spreadChange, spreadScalar);
        if (spreadScalar < 0 || spreadScalar > 1)
            spreadScalar = Mathf.Clamp(spreadScalar, 0, 1);
        if (spreadA != stat.spread && spreadScalar == 0)
            spreadA = stat.spread;
        gunTimer++;
    }
    IEnumerator Burst(int times, float delay)
    {
        inBurst = true;
        for (int b = times; b > 0; b--)
        {
            for (int i = stat.amount + extraShot; i > 0; i--)
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
        switch (stat.bulletType)
        {
            case BulletType.Bullet:
                StartCoroutine(ShootBullet());
                break;
            case BulletType.Laser:
                if (!laserActive) StartCoroutine(ShootLaser());
                break;
            case BulletType.Wave:
                break;
            case BulletType.Rocket:
                break;
            case BulletType.Needle:
                break;
            case BulletType.Railgun:
                break;
            case BulletType.Mine:
                break;
            case BulletType.Hammer:
                break;
            case BulletType.Cluster:
                break;
            case BulletType.Arrow:
                break;
            case BulletType.Mirage:
                break;
            case BulletType.Grand:
                break;
            case BulletType.Void:
                break;
            case BulletType.Beam:
                break;
            case BulletType.Blade:
                break;
            default:
                break;
        }
    }
    public IEnumerator ShootBullet()
    {
        GameObject bullet = Instantiate(stat.bulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = gc.bc;
        bullet.transform.localScale *= stat.bulletSize;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * Speed(stat.speed);
        gunTimer = 0;
        gunMaster.hasFired = true;
        yield return null;
    }

    public IEnumerator ShootLaser()
    {
        laserActive = true;
        LineRenderer laser = Instantiate(stat.laserPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation, bulletSpawnPoint).GetComponent<LineRenderer>();
        ParticleSystem laserHitPs = Instantiate(stat.laserPsPrefab).GetComponent<ParticleSystem>();
        var emis = laserHitPs.emission;
        while (Input.GetMouseButton(0))
        {
            //Calculate laser and hit
            RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.up, stat.laserRange, laserRayHitMask);
            Damagable dm = null;
            if (hit && hit.collider.GetComponent<Damagable>())
                dm = hit.collider.GetComponent<Damagable>();
            if (hit && dm) 
                dm.TakeDamage(stat.damage * Time.deltaTime * fireRateA);
            

            //Viusal effects
            if (!hit)
            {
                laser.SetPosition(1, new Vector2(0, 1) * stat.laserRange);
                emis.enabled = false;
            }
            else
            {
                laser.SetPosition(1, bulletSpawnPoint.InverseTransformPoint(hit.point));
                laserHitPs.transform.position = hit.point;
                laserHitPs.transform.LookAt(bulletSpawnPoint);
                emis.enabled = true;
            }

            yield return new WaitForEndOfFrame();
        }
        Destroy(laser.gameObject);
        emis.enabled = false;
        Destroy(laserHitPs.gameObject, 0.5f);
        laserActive = false;
    }

    private void Update()
    {
        Debug.DrawRay(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.up);
    }
    float Speed(float baseSpeed)
    {
        float spreadSpeed = baseSpeed;
        if (stat.amount > 1)
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
