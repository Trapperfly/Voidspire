using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class AIGun : MonoBehaviour
{
    [SerializeField] int idRelation;
    [SerializeField] bool isOnGun;
    [SerializeField] ShipAI ai;
    [HideInInspector] public AIEquipGun stat;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] bool autoFire;
    Transform bh;
    Transform hbh;
    float gunTimer = 0;
    float damageScaling = 0;
    float attackSpeedScaling = 0;

    private void Awake()
    {
        if (ai.guns.Length != 0) stat = ai.guns[idRelation];
    }
    private void Start()
    {
        if (!stat) return;
        bh = EnemyManager.Instance.bh;
        hbh = EnemyManager.Instance.hbh;
        float tempDamage = stat.damage;
        for (int i = 0; i < ai.level; i++)
        {
            tempDamage *= 1 + Difficulty.dif.AIDamageIncreasePerLevel;
        }
        damageScaling = tempDamage;
        float tempAS = stat.fireRate;
        for (int i = 0; i < ai.level; i++)
        {
            tempAS *= 1 + Difficulty.dif.AIFireRateIncreasePerLevel;
        }
        attackSpeedScaling = tempAS;
    }

    private void FixedUpdate()
    {
        if (!stat) return;
        if (autoFire)
        {
            float fireRate = 
                ai.doubleAttackSpeed 
                ? stat.fireRate 
                * ai.ship.enrageStrength 
                * attackSpeedScaling
                : stat.fireRate 
                * attackSpeedScaling;
            if (ai.inCombat && gunTimer >= 60 / fireRate) 
            { 
                for (int i = 0; i < stat.amount; i++) { Fire(); }
            }
            gunTimer++;
        }
    }

    public void Fire()
    {
        if (stat.homing) { bh = EnemyManager.Instance.hbh; }
        else bh = EnemyManager.Instance.bh;
        GameObject bullet = null;
        switch (stat.attack)
        {
            case AIAttack.None:
                break;
            case AIAttack.Bullet:
                bullet = Instantiate(EnemyManager.Instance.bulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.VoidSphere:
                bullet = Instantiate(EnemyManager.Instance.VoidSpherePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.FireMissile:
                bullet = Instantiate(EnemyManager.Instance.MissilePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.ElectricRailgun:
                bullet = Instantiate(EnemyManager.Instance.RailgunPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.Flak:
                bullet = Instantiate(EnemyManager.Instance.FlakPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.RapidFire:
                bullet = Instantiate(EnemyManager.Instance.RapidPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.Cannon:
                bullet = Instantiate(EnemyManager.Instance.CannonPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.SpawnEnemy:
                break;
            case AIAttack.LayMine:
                break;
            default:
                return;
        }
        if( bullet == null ) { return; }
        Physics2D.IgnoreCollision(GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
        AIBullet b = bullet.GetComponent<AIBullet>();
        b.damage =
            ai.doubleDamage ? 
            ai.ship.enrageStrength * damageScaling
            : 
            damageScaling;
        b.speed = stat.shotSpeed;
        b.homing = stat.homing;
        b.homingStrength = stat.homingStrength;

        b.target = ai.target.target;
        if (isOnGun) b.bulletSender = transform.parent.gameObject;
        else b.bulletSender = gameObject;

        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * stat.shotSpeed, ForceMode2D.Impulse);
        gunTimer = 0f;
    }

    Quaternion Spread(Quaternion baseRotation)
    {
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-stat.spread, stat.spread));
        return spreadValue;
    }
}
