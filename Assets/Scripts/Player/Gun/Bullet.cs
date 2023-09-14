using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class Bullet : MonoBehaviour
{
    public BulletController bc;
    public float _localDamage;
    public int _localBounce;
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
        _localBounce = bc.bounce;
        _localPierce = bc.pierce;
        _localHoming = bc.homing;
        _localDamage = bc.damage;
    }
    private void Start()
    {
        if (bc.bounce > 0)
        {
            col.isTrigger = false;
        }
    }
    private void FixedUpdate()
    {
        if (bounced)
        {
            col.isTrigger = false;
            bounced = false;
        }
        if (bc.sizeChange != 0)
        {
            float _sizeChangeValue = bc.sizeChange / 100;
            transform.localScale += new Vector3(_sizeChangeValue, _sizeChangeValue, 0);
        }
        if (bc.damageChange != 0)
        {
            _localDamage += bc.damageChange / 100;
        }
        if (bc.speedChange != 0)
        {
            rb.AddForce(bc.speedChange * bc.weightScalar * transform.up, ForceMode2D.Force);
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
}