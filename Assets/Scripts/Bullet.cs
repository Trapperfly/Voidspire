using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public float _damageChange;
    public float _sizeChange;
    public float _speed;
    public float _bulletLongevity;
    public int _pierce;
    public int _bounce;
    public bool _bounceToTarget;
    public bool _homing;
    public float _homingStrength;
    public float _punch;
    float currTime;
    Rigidbody2D rb;
    public GameObject target;

    private void Awake()
    {
        currTime = Time.time;
        rb = GetComponent<Rigidbody2D>();

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
            transform.up = rb.velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.GetComponent<Rigidbody2D>() != null)
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * _punch);
            else if (collision.GetComponentInChildren<Rigidbody2D>() != null)
                collision.GetComponentInChildren<Rigidbody2D>().AddForce(transform.up * _punch, ForceMode2D.Impulse);
            if (_pierce > 0)
                _pierce--;
            else if (_bounce > 0)
            {
                Vector2 _hit = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)collision.transform.position;
                Vector2 _bounceDir = Vector2.Reflect(rb.velocity.normalized, _hit.normalized).normalized;
                rb.velocity = _bounceDir * _speed;
                _bounce--;
            }
            else
                Destroy(gameObject);
        }
    }
}