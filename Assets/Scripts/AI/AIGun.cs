using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGun : MonoBehaviour
{
    [SerializeField] bool isOnGun;
    [SerializeField] ShipAI ai;
    [HideInInspector] public Ship stat;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] bool autoFire;
    Transform bh;
    float gunTimer = 0;

    private void Awake()
    {
        stat = ai.ship;
        bh = GameObject.FindGameObjectWithTag("AIBulletHolder").transform;
    }
    private void FixedUpdate()
    {
        if (autoFire)
        {
            if (ai.inCombat && gunTimer >= 60 / stat.fireRate) { Fire(); }
            gunTimer++;
        }
    }

    public void Fire()
    {
        GameObject bullet = null;
        switch (stat.normalAttack)
        {
            case NormalAttack.None:
                break;
            case NormalAttack.Bullet:
                bullet = Instantiate(EnemyManager.Instance.bulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case NormalAttack.VoidSphere:
                bullet = Instantiate(EnemyManager.Instance.VoidSpherePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case NormalAttack.FireMissile:
                bullet = Instantiate(EnemyManager.Instance.MissilePrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            case NormalAttack.ElectricRailgun:
                bullet = Instantiate(EnemyManager.Instance.RailgunPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
                break;
            default:
                return;
        }
        if( bullet == null ) { return; }
        Physics2D.IgnoreCollision(GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
        bullet.GetComponent<AIBullet>().damage = stat.damage;
        if (isOnGun) bullet.GetComponent<AIBullet>().bulletSender = transform.parent.gameObject;
        else bullet.GetComponent<AIBullet>().bulletSender = gameObject;

        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * stat.shotSpeed, ForceMode2D.Impulse);
        gunTimer = 0f;
    }

    Quaternion Spread(Quaternion baseRotation)
    {
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-stat.spread, stat.spread));
        return spreadValue;
    }
}
