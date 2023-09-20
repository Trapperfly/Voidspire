using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    GunStats stat;
    GunController control;
    Transform bulletHolder;
    [SerializeField] BulletController bc;
    [SerializeField] AdjustToTarget target;
    [SerializeField] Transform bulletSpawnPoint;

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
        control = GetComponent<GunController>();
        bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
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
                        StartCoroutine(Shoot());
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
                StartCoroutine(Shoot());
                charge = 0;
                chargeAvailable = false;
            }
            yield return new WaitForSeconds(delay);
        }
        yield return null;
        inBurst = false;
        extraShot = 0;
    }
    public IEnumerator Shoot()
    {
        GameObject bullet = Instantiate(stat.bulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bulletHolder);
        bullet.GetComponent<Bullet>().bc = bc;
        bullet.transform.localScale *= stat.bulletSize;
        bullet.GetComponent<Rigidbody2D>().AddForce(Speed(stat.speed) * stat.weightScalar * bullet.transform.up, ForceMode2D.Impulse);
        gunTimer = 0;
        yield return null;
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
