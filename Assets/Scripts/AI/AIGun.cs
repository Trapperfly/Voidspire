using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using FMODUnity;

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

    GameObject spawnedEnemy;
    public float gunTimerOffset;

    StudioEventEmitter fireProjectile;
    StudioEventEmitter fireProjectileLight;
    StudioEventEmitter fireProjectileMedium;
    StudioEventEmitter fireProjectileHeavy;
    StudioEventEmitter fireProjectileBig;
    StudioEventEmitter fireProjectileMissile;

    private void Awake()
    {
        if (ai.guns.Length != 0) stat = ai.guns[idRelation];
    }
    private void Start()
    {
        gunTimer -= gunTimerOffset;
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
            if (ai.inCombat) { gunTimer++; }
        }
    }

    public void Fire()
    {
        if (stat.homing) { bh = EnemyManager.Instance.hbh; }
        else bh = EnemyManager.Instance.bh;
        GameObject bullet = null;
        bool isSpawnedEnemy = false;
        switch (stat.attack)
        {
            case AIAttack.None:
                break;
            case AIAttack.Bullet:
                bullet = Instantiate(EnemyManager.Instance.bulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);

                break;
            case AIAttack.VoidSphere:
                bullet = Instantiate(EnemyManager.Instance.VoidSpherePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                fireProjectileHeavy = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireHeavyProjectile, gameObject);
                fireProjectileHeavy.Play();
                break;
            case AIAttack.FireMissile:
                bullet = Instantiate(EnemyManager.Instance.MissilePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                fireProjectileMissile = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireMissile, gameObject);
                fireProjectileMissile.Play();
                break;
            case AIAttack.ElectricRailgun:
                bullet = Instantiate(EnemyManager.Instance.RailgunPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case AIAttack.Flak:
                bullet = Instantiate(EnemyManager.Instance.FlakPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                fireProjectile = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireProjectile, gameObject);
                fireProjectile.Play();
                break;
            case AIAttack.RapidFire:
                bullet = Instantiate(EnemyManager.Instance.RapidPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                fireProjectileLight = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireLightProjectile, gameObject);
                fireProjectileLight.Play();
                break;
            case AIAttack.Cannon:
                bullet = Instantiate(EnemyManager.Instance.CannonPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                fireProjectileMedium = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireMediumProjectile, gameObject);
                fireProjectileMedium.Play();
                break;
            case AIAttack.SpawnEnemy:
                bullet = Instantiate(EnemyManager.Instance.enemySpawnPrefab, bulletSpawnPoint.position, Spread(transform.rotation), SpawnEnemies.Instance.transform);
                Debug.LogError("Spawned enemy");
                isSpawnedEnemy = true;
                fireProjectileBig = AudioManager.Instance.InitEmitter(FMODEvents.Instance.enemyFireBigProjectile, gameObject);
                fireProjectileBig.Play();
                break;
            case AIAttack.LayMine:
                break;
            default:
                return;
        }
        if ( bullet == null ) { return; }
        if ( isSpawnedEnemy ) {
            bullet.GetComponent<ShipAI>().idle = true;
            bullet.GetComponent<Collider2D>().enabled = false;
            spawnedEnemy = bullet;
            Invoke(nameof(ActivateSpawnedEnemy), 0.5f);
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * stat.shotSpeed, ForceMode2D.Impulse);
            gunTimer = 0f;
            return;
        }
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

    void ActivateSpawnedEnemy()
    {
        spawnedEnemy.GetComponent<ShipAI>().idle = false;
        spawnedEnemy.GetComponent<ShipAI>().inCombat = true;
        spawnedEnemy.GetComponent<ShipAI>().target.target = ai.target.target;
        spawnedEnemy.GetComponent<Collider2D>().enabled = true;
    }

    Quaternion Spread(Quaternion baseRotation)
    {
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-stat.spread, stat.spread));
        return spreadValue;
    }
}
