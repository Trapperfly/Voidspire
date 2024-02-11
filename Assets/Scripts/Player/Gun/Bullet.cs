using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class Bullet : MonoBehaviour
{
    public BulletController bc;
    public GameObject bulletSender;
    public float _localDamage;
    public float _splashDamage;
    public float _splashRange;
    public int _localBounce;
    public int _localPierce;
    public bool _localHoming;
    public bool _explosive;
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
        _localBounce = bc.bounce;
        _localPierce = bc.pierce;
        _localHoming = bc.homing;
        _localDamage = bc.damage;
        _splashDamage = bc.splashDamage;
        _splashRange = bc.splashRange;
        _explosive = bc.isExplosive;
    }
    private void Start()
    {
        if (!_explosive && bc.bounce > 0)
        {
            col.isTrigger = false;
        }
    }
    private void FixedUpdate()
    {
        if (bounced)
        {
            _localBounce--;
            col.isTrigger = false;
            bounced = false;
        }
        lastVelocity = rb.velocity;     //Logs last velocity to be used in bounce
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
        if (_explosive)
        {
            Debug.Log(_splashDamage);
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _splashRange);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<Damagable>(out var dm)) dm.TakeDamage(_splashDamage, col.transform.position, bulletSender);
            }
        }
        StopAllCoroutines();
    }
}