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

    private void Awake()
    {
        stat = ai.guns[idRelation];
    }
    private void Start()
    {
        bh = EnemyManager.Instance.bh;
        hbh = EnemyManager.Instance.hbh;
    }

    private void FixedUpdate()
    {
        if (autoFire)
        {
            float fireRate = 
                ai.doubleAttackSpeed 
                ? stat.fireRate 
                * ai.ship.enrageStrength 
                * (1 + (ai.level * Difficulty.dif.AIFireRateIncreasePerLevel))
                : stat.fireRate 
                
                * (1 + (ai.level * Difficulty.dif.AIFireRateIncreasePerLevel));
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
            default:
                return;
        }
        if( bullet == null ) { return; }
        Physics2D.IgnoreCollision(GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
        AIBullet b = bullet.GetComponent<AIBullet>();
        b.damage = ai.doubleDamage 
            ? stat.damage * ai.ship.enrageStrength *
            ai.level * Difficulty.dif.AIDamageIncreasePerLevel
            : stat.damage * ai.level * Difficulty.dif.AIDamageIncreasePerLevel;
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
