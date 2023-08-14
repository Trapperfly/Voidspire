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
    Vector3 lastVelocity;

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
        lastVelocity = rb.velocity;     //Logs last velocity to be used in bounce
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.collider.GetComponent<Rigidbody2D>() != null)
                collision.collider.GetComponent<Rigidbody2D>().AddForce(transform.up * _punch);
            else if (collision.collider.GetComponentInChildren<Rigidbody2D>() != null)
                collision.collider.GetComponentInChildren<Rigidbody2D>().AddForce(transform.up * _punch, ForceMode2D.Impulse);
            if (_pierce > 0)
            {
                rb.velocity = lastVelocity;
                _pierce--;
            }
            else if (_bounce > 0)
            {
                /*
                Vector2 _hit = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)collision.transform.position;
                Vector2 _bounceDir = Vector2.Reflect(rb.velocity.normalized, _hit.normalized).normalized;
                rb.velocity = _bounceDir * _speed;
                */
                foreach (var contact in collision.contacts)
                {
                    Vector2 _bounceDir = Vector2.Reflect(lastVelocity, contact.normal).normalized;
                    rb.velocity = _bounceDir * _speed;
                    rb.angularVelocity = 0;
                }
                _bounce--;
            }
            else
                Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}