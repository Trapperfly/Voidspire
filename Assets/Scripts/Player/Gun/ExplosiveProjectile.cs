using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public BulletController bc;
    public float _localDamage;
    public float _splashDamage;
    public float _splashRange;
    public int _localPierce;
    public bool _localHoming;
    public float currTime;
    public Rigidbody2D rb;
    Collider2D col;

    public Vector3 lastVelocity;
    public bool bounced = false;

    private void Awake()
    {
        currTime = Time.time;
        bc = GetComponentInParent<BulletController>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        _localPierce = bc.pierce;
        _localHoming = bc.homing;
        _localDamage = bc.damage;
        _splashDamage = bc.damage * 10;
        _splashRange = bc.splashRange;
    }
    private void Start()
    {
        if (bc.bounce > 0)
        {
            col.isTrigger = false;
        }
    }
    public IEnumerator WaitAndSwitchHoming(float time)
    {
        _localHoming = false;
        yield return new WaitForSeconds(time * bc.homingStrength / 25);
        _localHoming = true;
        yield return new WaitForFixedUpdate();
    }
    private void OnDestroy()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _splashRange);
        foreach (Collider2D col in hit)
        {
            if (col.TryGetComponent<Damagable>(out var dm)) dm.TakeDamage(_splashDamage, col.transform.position);
        }
        StopAllCoroutines();
    }
}
