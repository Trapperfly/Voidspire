using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGun : MonoBehaviour
{
    [SerializeField] LurkerAI aiLurker;
    [SerializeField] AIGunStats stat;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] bool autoFire;
    Transform bh;
    float gunTimer;

    private void Awake()
    {
        bh = GameObject.FindGameObjectWithTag("AIBulletHolder").transform;
    }
    private void FixedUpdate()
    {
        if (autoFire)
        {
            if (aiLurker.inCombat && gunTimer >= 60 / stat.fireRate) Fire();
            gunTimer++;
        }
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(stat.AIBulletPrefab, bulletSpawnPoint.position, Spread(transform.rotation), bh);
        Physics2D.IgnoreCollision(GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
        bullet.GetComponent<AIBullet>().damage = stat.damage;
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * stat.speed, ForceMode2D.Impulse);
        gunTimer = 0f;
    }

    Quaternion Spread(Quaternion baseRotation)
    {
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-stat.spread, stat.spread));
        return spreadValue;
    }
}
