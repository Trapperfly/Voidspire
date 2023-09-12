using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public float _damageChange;
    public float _sizeChange;
    public float _speed;
    public float _speedChange;
    public float _bulletLongevity;
    public int _pierce;
    public int _bounce;
    public bool _bounceToTarget;
    public bool _homing;
    public float _homingStrength;
    public float _punch;
    float currTime;
    Rigidbody2D rb;
    Collider2D col;
    public GameObject target;
    public Vector3 lastVelocity;
    public bool bounced = false;
    public float _weightScalar;
    private void Awake()
    {
        currTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    private void Start()
    {
        if (_bounce > 0)
        {
            col.isTrigger = false;
        }
    }
    void Update()
    {
        if (currTime <= Time.time - _bulletLongevity)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (_homing && target != null)
        {
            Vector2 direction = (Vector2)target.transform.position - rb.position;
            float rotateAmount = Vector3.Cross(direction.normalized, transform.up).z;
            rb.angularVelocity = -_homingStrength * rotateAmount;
            rb.velocity = transform.up * _speed;
            /*
            Vector2 direction = (target.transform.position - transform.position)- transform.up;
            rb.AddForce(direction.normalized * _homingStrength, ForceMode2D.Force);
            //*(target.transform.position - transform.position).magnitude
            transform.up = rb.velocity;
            */
        }
        else
        {
            transform.up = rb.velocity;
            rb.angularVelocity = 0;
        }
        if (bounced)
        {
            col.isTrigger = false;
            bounced = false;
        }
        if (_sizeChange != 0)
        {
            float _sizeChangeValue = _sizeChange / 100;
            transform.localScale += new Vector3(_sizeChangeValue, _sizeChangeValue, 0);
        }
        if (_damageChange != 0)
        {
            _damage += _damageChange / 100;
        }
        if (_speedChange != 0)
        {
            rb.AddForce(_speedChange * 0.0001f * transform.up, ForceMode2D.Force);
        }
        lastVelocity = rb.velocity;     //Logs last velocity to be used in bounce
    }
    public IEnumerator WaitAndSwitchHoming(float time)
    {
        _homing = false;
        yield return new WaitForSeconds(time * _homingStrength / 25);
        _homing = true;
        yield return new WaitForFixedUpdate();
    }
}